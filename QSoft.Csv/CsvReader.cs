using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace QSoft.Csv
{
    public class CsvReader : IDisposable
    {
        private readonly StreamReader _fileStream;
        private char[] _buffer;
        private int _bufferLength;
        private int _bufferPosition;
        private readonly int _bufferSize;
        private bool _endOfStream;
        private bool _skipNextLF;
        private readonly List<Range> _ranges = [];
        public CsvReader(Stream stream, int bufferSize = 16)
        {
            _fileStream = new StreamReader(stream, Encoding.UTF8);
            _bufferSize = bufferSize;
            _buffer = ArrayPool<char>.Shared.Rent(bufferSize);
            _bufferLength = 0;
            _bufferPosition = 0;
        }


        public bool Read(out CsvColumns record)
        {
            record = default;

            if (_skipNextLF)
            {
                _skipNextLF = false;
                if (_bufferPosition >= _bufferLength && !_endOfStream)
                {
                    RefillBuffer(_bufferPosition);
                    _bufferPosition = 0;
                }
                if (_bufferPosition < _bufferLength && _buffer[_bufferPosition] == '\n')
                    _bufferPosition++;
            }

            if (_endOfStream && _bufferPosition >= _bufferLength)
                return false;

            _ranges.Clear();
            int recordStart = _bufferPosition;
            int columnRelStart = 0;
            bool inQuotes = false;

            while (true)
            {
                if (_bufferPosition >= _bufferLength)
                {
                    if (_endOfStream)
                    {
                        int lineLen = _bufferPosition - recordStart;
                        if (lineLen > 0 || _ranges.Count > 0)
                        {
                            AddColumnRange(_ranges, recordStart, columnRelStart, lineLen);
                            var line = _buffer.AsSpan(recordStart, lineLen);
                            record = new CsvColumns(line, CollectionsMarshal.AsSpan(_ranges));
                            return true;
                        }
                        return false;
                    }

                    int recordLen = _bufferPosition - recordStart;
                    EnsureBufferCapacity(recordLen + _bufferSize);
                    RefillBuffer(recordStart);
                    _bufferPosition = recordLen;
                    recordStart = 0;
                    continue;
                }

                char ch = _buffer[_bufferPosition];
                int relPos = _bufferPosition - recordStart;

                if (ch == '"')
                {
                    inQuotes = !inQuotes;
                    _bufferPosition++;
                }
                else if (ch == ',' && !inQuotes)
                {
                    AddColumnRange(_ranges, recordStart, columnRelStart, relPos);
                    columnRelStart = relPos + 1;
                    _bufferPosition++;
                }
                else if ((ch == '\r' || ch == '\n') && !inQuotes)
                {
                    int lineLen = relPos;
                    _bufferPosition++;

                    if (ch == '\r')
                    {
                        if (_bufferPosition < _bufferLength)
                        {
                            if (_buffer[_bufferPosition] == '\n')
                                _bufferPosition++;
                        }
                        else if (!_endOfStream)
                        {
                            _skipNextLF = true;
                        }
                    }

                    AddColumnRange(_ranges, recordStart, columnRelStart, lineLen);
                    var line = _buffer.AsSpan(recordStart, lineLen);
                    record = new (line, CollectionsMarshal.AsSpan(_ranges));
                    return true;
                }
                else
                {
                    _bufferPosition++;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddColumnRange(List<Range> ranges, int recordStart, int columnStart, int columnEnd)
        {
            int start = columnStart;
            int end = columnEnd;
            int length = end - start;

            if (length >= 2)
            {
                int absStart = recordStart + start;
                int absEnd = recordStart + end - 1;
                
                if (absStart < _bufferLength && absEnd < _bufferLength &&
                    _buffer[absStart] == '"' && _buffer[absEnd] == '"')
                {
                    start++;
                    end--;
                }
            }

            ranges.Add(new Range(start, end));
        }

        private void EnsureBufferCapacity(int required)
        {
            if (_buffer.Length >= required)
                return;

            int newSize = Math.Max(_buffer.Length * 2, required);
            char[] newBuffer = ArrayPool<char>.Shared.Rent(newSize);
            Array.Copy(_buffer, newBuffer, _bufferLength);
            ArrayPool<char>.Shared.Return(_buffer);
            _buffer = newBuffer;
        }

        private void RefillBuffer(int preserveFrom)
        {
            if (preserveFrom >= _bufferLength)
            {
                _bufferLength = 0;
                _bufferPosition = 0;
            }
            else if (preserveFrom > 0)
            {
                int remaining = _bufferLength - preserveFrom;
                Array.Copy(_buffer, preserveFrom, _buffer, 0, remaining);
                _bufferLength = remaining;
                _bufferPosition = 0;
            }

            int available = _buffer.Length - _bufferLength;
            if (available > 0)
            {
                int bytesRead = _fileStream.Read(_buffer, _bufferLength, available);
                _bufferLength += bytesRead;
                if (bytesRead == 0)
                    _endOfStream = true;
            }
        }

        public void Dispose()
        {
            _fileStream?.Dispose();
            if (_buffer != null)
            {
                ArrayPool<char>.Shared.Return(_buffer);
                _buffer = null;
            }
            
        }
    }

    public readonly ref struct CsvColumns
    {
        private readonly ReadOnlySpan<char> _line;
        private readonly ReadOnlySpan<Range> _ranges;

        internal CsvColumns(ReadOnlySpan<char> line, ReadOnlySpan<Range> ranges)
        {
            _line = line;
            _ranges = ranges;
        }

        public int Length => _ranges.Length;

        /// <summary>取得指定索引的欄位內容（零複製）</summary>
        public ReadOnlySpan<char> this[int index] => _line[_ranges[index]];
    }

    public static class CsvReaderExtensions
    {
        public static int? ToInt(this ReadOnlySpan<char> span)
        {
            if (int.TryParse(span, out var result))
                return result;
            return null;
        }

        public static double? ToDouble(this ReadOnlySpan<char> span)
        {
            if (double.TryParse(span, out var result))
                return result;
            return null;
        }
    }
}