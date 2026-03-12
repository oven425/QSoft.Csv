using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace QSoft.Csv
{
    public class CsvReader : IDisposable
    {
        private readonly StreamReader _fileStream;
        private readonly char[] _buffer;
        private int _bufferLength;
        private int _bufferPosition;
        private readonly int _bufferSize;
        private bool _endOfStream;

        public CsvReader(string filePath, int bufferSize = 4096)
        {
            _fileStream = new StreamReader(filePath);
            _bufferSize = bufferSize;
            _buffer = ArrayPool<char>.Shared.Rent(bufferSize);
            _bufferLength = 0;
            _bufferPosition = 0;
        }

        /// <summary>
        /// 讀取一行數據（支援 \r, \n, \r\n）
        /// 返回 Span 和換行符結束位置
        /// </summary>
        public bool Readline(out ReadOnlySpan<char> line)
        {
            line = default;
            int lineStart = _bufferPosition;

            while (true)
            {
                if (_bufferPosition >= _bufferLength)
                {
                    if (_endOfStream)
                    {
                        if (lineStart < _bufferLength)
                        {
                            line = _buffer.AsSpan(lineStart, _bufferLength - lineStart);
                            _bufferPosition = _bufferLength;
                            return true;
                        }
                        return false;
                    }

                    // 讀取更多數據
                    RefillBuffer(lineStart);
                    if (_bufferLength == 0) return false;
                    lineStart = 0;
                }

                var current = _buffer[_bufferPosition];

                // 檢查換行符
                if (current == '\n')
                {
                    line = _buffer.AsSpan(lineStart, _bufferPosition - lineStart);
                    _bufferPosition++;
                    return true;
                }

                if (current == '\r')
                {
                    int lineLen = _bufferPosition - lineStart;
                    _bufferPosition++;

                    // 檢查是否是 \r\n
                    if (_bufferPosition < _bufferLength && _buffer[_bufferPosition] == '\n')
                    {
                        _bufferPosition++;
                    }

                    line = _buffer.AsSpan(lineStart, lineLen);
                    return true;
                }

                _bufferPosition++;
            }
        }

        /// <summary>
        /// 分割列，返回每列的 Range
        /// 假設每列都用雙引號包起來
        /// </summary>
        public List<Range> SplitColumn(ReadOnlySpan<char> line)
        {
            var ranges = new List<Range>();
            int columnStart = 0;
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                var current = line[i];

                if (current == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (current == ',' && !inQuotes)
                {
                    // 找到列分隔符
                    ranges.Add(new Range(columnStart+1, i-1));
                    columnStart = i + 1;
                }
            }

            // 加入最後一列
            if (columnStart < line.Length)
            {
                ranges.Add(new Range(columnStart, line.Length));
            }

            return ranges;
        }

        private void RefillBuffer(int preserveFrom)
        {
            // 如果需要，移動未讀資料到緩衝區開始
            if (preserveFrom > 0 && preserveFrom < _bufferLength)
            {
                int remaining = _bufferLength - preserveFrom;
                Array.Copy(_buffer, preserveFrom, _buffer, 0, remaining);
                _bufferLength = remaining;
                _bufferPosition = 0;
            }
            else
            {
                _bufferLength = 0;
                _bufferPosition = 0;
            }

            // 讀取新數據
            int bytesRead = _fileStream.Read(_buffer, _bufferLength, _bufferSize - _bufferLength);
            _bufferLength += bytesRead;

            if (bytesRead == 0)
            {
                _endOfStream = true;
            }
        }

        public void Dispose()
        {
            _fileStream?.Dispose();
            ArrayPool<char>.Shared.Return(_buffer);
        }
    }
}