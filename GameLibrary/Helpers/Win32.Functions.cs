using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace GameLibrary.Helpers
{
    /// <summary>
    /// Helper Win32 methods
    /// </summary>
    public static partial class Win32
    {
        public static partial class ConsoleLibrary
        {
            // http://pinvoke.net/default.aspx/kernel32/AddConsoleAlias.html
            [DllImport("kernel32", SetLastError = true)]
            public static extern bool AddConsoleAlias(
                string Source,
                string Target,
                string ExeName
                );

            // http://pinvoke.net/default.aspx/kernel32/AllocConsole.html
            [DllImport("kernel32", SetLastError = true)]
            public static extern bool AllocConsole();

            // http://pinvoke.net/default.aspx/kernel32/AttachConsole.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool AttachConsole(
                uint dwProcessId
                );

            // http://pinvoke.net/default.aspx/kernel32/CreateConsoleScreenBuffer.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr CreateConsoleScreenBuffer(
                uint dwDesiredAccess,
                uint dwShareMode,
                IntPtr lpSecurityAttributes,
                uint dwFlags,
                IntPtr lpScreenBufferData
                );

            // http://pinvoke.net/default.aspx/kernel32/FillConsoleOutputAttribute.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool FillConsoleOutputAttribute(
                IntPtr hConsoleOutput,
                ushort wAttribute,
                uint nLength,
                COORD dwWriteCoord,
                out uint lpNumberOfAttrsWritten
                );

            // http://pinvoke.net/default.aspx/kernel32/FillConsoleOutputCharacter.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool FillConsoleOutputCharacter(
                IntPtr hConsoleOutput,
                char cCharacter,
                uint nLength,
                COORD dwWriteCoord,
                out uint lpNumberOfCharsWritten
                );

            // http://pinvoke.net/default.aspx/kernel32/FlushConsoleInputBuffer.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool FlushConsoleInputBuffer(
                IntPtr hConsoleInput
                );

            // http://pinvoke.net/default.aspx/kernel32/FreeConsole.html
            [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
            public static extern bool FreeConsole();

            // http://pinvoke.net/default.aspx/kernel32/GenerateConsoleCtrlEvent.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GenerateConsoleCtrlEvent(
                uint dwCtrlEvent,
                uint dwProcessGroupId
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleAlias.html
            [DllImport("kernel32", SetLastError = true)]
            public static extern bool GetConsoleAlias(
                string Source,
                out StringBuilder TargetBuffer,
                uint TargetBufferLength,
                string ExeName
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleAliases.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern uint GetConsoleAliases(
                StringBuilder[] lpTargetBuffer,
                uint targetBufferLength,
                string lpExeName
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleAliasesLength.html
            [DllImport("kernel32", SetLastError = true)]
            public static extern uint GetConsoleAliasesLength(
                string ExeName
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleAliasExes.html
            [DllImport("kernel32", SetLastError = true)]
            public static extern uint GetConsoleAliasExes(
                out StringBuilder ExeNameBuffer,
                uint ExeNameBufferLength
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleAliasExesLength.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern uint GetConsoleAliasExesLength();

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleCP.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern uint GetConsoleCP();

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleCursorInfo.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GetConsoleCursorInfo(
                IntPtr hConsoleOutput,
                out CONSOLE_CURSOR_INFO lpConsoleCursorInfo
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleDisplayMode.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GetConsoleDisplayMode(
                out uint ModeFlags
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleFontSize.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern COORD GetConsoleFontSize(
                IntPtr hConsoleOutput,
                Int32 nFont
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleHistoryInfo.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GetConsoleHistoryInfo(
                out CONSOLE_HISTORY_INFO ConsoleHistoryInfo
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleMode.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GetConsoleMode(
                IntPtr hConsoleHandle,
                out uint lpMode
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleOriginalTitle.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern uint GetConsoleOriginalTitle(
                out StringBuilder ConsoleTitle,
                uint Size
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleOutputCP.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern uint GetConsoleOutputCP();

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleProcessList.html
            // TODO: Test - what's an out uint[] during interop? This probably isn't quite right, but provides a starting point:
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern uint GetConsoleProcessList(
                out uint[] ProcessList,
                uint ProcessCount
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleScreenBufferInfo.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GetConsoleScreenBufferInfo(
                IntPtr hConsoleOutput,
                out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleScreenBufferInfoEx.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GetConsoleScreenBufferInfoEx(
                IntPtr hConsoleOutput,
                ref CONSOLE_SCREEN_BUFFER_INFO_EX ConsoleScreenBufferInfo
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleSelectionInfo.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GetConsoleSelectionInfo(
                CONSOLE_SELECTION_INFO ConsoleSelectionInfo
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleTitle.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern uint GetConsoleTitle(
                [Out] StringBuilder lpConsoleTitle,
                uint nSize
                );

            // http://pinvoke.net/default.aspx/kernel32/GetConsoleWindow.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GetConsoleWindow();

            // http://pinvoke.net/default.aspx/kernel32/GetCurrentConsoleFont.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GetCurrentConsoleFont(
                IntPtr hConsoleOutput,
                bool bMaximumWindow,
                out CONSOLE_FONT_INFO lpConsoleCurrentFont
                );

            // http://pinvoke.net/default.aspx/kernel32/GetCurrentConsoleFontEx.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GetCurrentConsoleFontEx(
                IntPtr ConsoleOutput,
                bool MaximumWindow,
                out CONSOLE_FONT_INFO_EX ConsoleCurrentFont
                );

            // http://pinvoke.net/default.aspx/kernel32/GetLargestConsoleWindowSize.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern COORD GetLargestConsoleWindowSize(
                IntPtr hConsoleOutput
                );

            // http://pinvoke.net/default.aspx/kernel32/GetNumberOfConsoleInputEvents.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GetNumberOfConsoleInputEvents(
                IntPtr hConsoleInput,
                out uint lpcNumberOfEvents
                );

            // http://pinvoke.net/default.aspx/kernel32/GetNumberOfConsoleMouseButtons.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool GetNumberOfConsoleMouseButtons(
                ref uint lpNumberOfMouseButtons
                );

            // http://pinvoke.net/default.aspx/kernel32/GetStdHandle.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern IntPtr GetStdHandle(
                int nStdHandle
                );

            // http://pinvoke.net/default.aspx/kernel32/HandlerRoutine.html
            // Delegate type to be used as the Handler Routine for SCCH
            public delegate bool ConsoleCtrlDelegate(CtrlTypes CtrlType);

            // http://pinvoke.net/default.aspx/kernel32/PeekConsoleInput.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool PeekConsoleInput(
                IntPtr hConsoleInput,
                [Out] INPUT_RECORD[] lpBuffer,
                uint nLength,
                out uint lpNumberOfEventsRead
                );

            // http://pinvoke.net/default.aspx/kernel32/ReadConsole.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool ReadConsole(
                IntPtr hConsoleInput,
                [Out] StringBuilder lpBuffer,
                uint nNumberOfCharsToRead,
                out uint lpNumberOfCharsRead,
                IntPtr lpReserved
                );

            // http://pinvoke.net/default.aspx/kernel32/ReadConsoleInput.html
            [DllImport("kernel32.dll", EntryPoint = "ReadConsoleInputW", CharSet = CharSet.Unicode)]
            public static extern bool ReadConsoleInput(
                IntPtr hConsoleInput,
                [Out] INPUT_RECORD[] lpBuffer,
                uint nLength,
                out uint lpNumberOfEventsRead
                );

            // http://pinvoke.net/default.aspx/kernel32/ReadConsoleOutput.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool ReadConsoleOutput(
                IntPtr hConsoleOutput,
                [Out] CHAR_INFO[] lpBuffer,
                COORD dwBufferSize,
                COORD dwBufferCoord,
                ref SMALL_RECT lpReadRegion
                );

            // http://pinvoke.net/default.aspx/kernel32/ReadConsoleOutputAttribute.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool ReadConsoleOutputAttribute(
                IntPtr hConsoleOutput,
                [Out] ushort[] lpAttribute,
                uint nLength,
                COORD dwReadCoord,
                out uint lpNumberOfAttrsRead
                );

            // http://pinvoke.net/default.aspx/kernel32/ReadConsoleOutputCharacter.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool ReadConsoleOutputCharacter(
                IntPtr hConsoleOutput,
                [Out] StringBuilder lpCharacter,
                uint nLength,
                COORD dwReadCoord,
                out uint lpNumberOfCharsRead
                );

            // http://pinvoke.net/default.aspx/kernel32/ScrollConsoleScreenBuffer.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool ScrollConsoleScreenBuffer(
                IntPtr hConsoleOutput,
               [In] ref SMALL_RECT lpScrollRectangle,
                IntPtr lpClipRectangle,
               COORD dwDestinationOrigin,
                [In] ref CHAR_INFO lpFill
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleActiveScreenBuffer.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleActiveScreenBuffer(
                IntPtr hConsoleOutput
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleCP.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleCP(
                uint wCodePageID
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleCtrlHandler.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleCtrlHandler(
                ConsoleCtrlDelegate HandlerRoutine,
                bool Add
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleCursorInfo.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleCursorInfo(
                IntPtr hConsoleOutput,
                [In] ref CONSOLE_CURSOR_INFO lpConsoleCursorInfo
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleCursorPosition.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleCursorPosition(
                IntPtr hConsoleOutput,
               COORD dwCursorPosition
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleDisplayMode.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleDisplayMode(
                IntPtr ConsoleOutput,
                uint Flags,
                out COORD NewScreenBufferDimensions
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleHistoryInfo.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleHistoryInfo(
                CONSOLE_HISTORY_INFO ConsoleHistoryInfo
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleMode.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleMode(
                IntPtr hConsoleHandle,
                uint dwMode
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleOutputCP.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleOutputCP(
                uint wCodePageID
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleScreenBufferInfoEx.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleScreenBufferInfoEx(
                IntPtr ConsoleOutput,
                CONSOLE_SCREEN_BUFFER_INFO_EX ConsoleScreenBufferInfoEx
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleScreenBufferSize.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleScreenBufferSize(
                IntPtr hConsoleOutput,
                COORD dwSize
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleTextAttribute.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleTextAttribute(
                IntPtr hConsoleOutput,
               ushort wAttributes
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleTitle.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleTitle(
                string lpConsoleTitle
                );

            // http://pinvoke.net/default.aspx/kernel32/SetConsoleWindowInfo.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetConsoleWindowInfo(
                IntPtr hConsoleOutput,
                bool bAbsolute,
                [In] ref SMALL_RECT lpConsoleWindow
                );

            // http://pinvoke.net/default.aspx/kernel32/SetCurrentConsoleFontEx.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetCurrentConsoleFontEx(
                IntPtr ConsoleOutput,
                bool MaximumWindow,
                CONSOLE_FONT_INFO_EX ConsoleCurrentFontEx
                );

            // http://pinvoke.net/default.aspx/kernel32/SetStdHandle.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool SetStdHandle(
                uint nStdHandle,
                IntPtr hHandle
                );

            // http://pinvoke.net/default.aspx/kernel32/WriteConsole.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool WriteConsole(
                IntPtr hConsoleOutput,
                string lpBuffer,
                uint nNumberOfCharsToWrite,
                out uint lpNumberOfCharsWritten,
                IntPtr lpReserved
                );

            // http://pinvoke.net/default.aspx/kernel32/WriteConsoleInput.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool WriteConsoleInput(
                IntPtr hConsoleInput,
                INPUT_RECORD[] lpBuffer,
                uint nLength,
                out uint lpNumberOfEventsWritten
                );

            // http://pinvoke.net/default.aspx/kernel32/WriteConsoleOutput.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool WriteConsoleOutput(
                IntPtr hConsoleOutput,
                CHAR_INFO[] lpBuffer,
                COORD dwBufferSize,
                COORD dwBufferCoord,
                ref SMALL_RECT lpWriteRegion
                );

            // http://pinvoke.net/default.aspx/kernel32/WriteConsoleOutputAttribute.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool WriteConsoleOutputAttribute(
                IntPtr hConsoleOutput,
                ushort[] lpAttribute,
                uint nLength,
                COORD dwWriteCoord,
                out uint lpNumberOfAttrsWritten
                );

            // http://pinvoke.net/default.aspx/kernel32/WriteConsoleOutputCharacter.html
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool WriteConsoleOutputCharacter(
                IntPtr hConsoleOutput,
                string lpCharacter,
                uint nLength,
                COORD dwWriteCoord,
                out uint lpNumberOfCharsWritten
                );

            [DllImport("kernel32.dll")]
            public static extern bool SetStdHandle(StdHandle nStdHandle, IntPtr hHandle);

            public enum StdHandle : int
            {
                Input = -10,
                Output = -11,
                Error = -12
            }

            public static IntPtr GetConsoleStandardInput()
            {
                var handle = CreateFile
                    ("CONIN$"
                    , EFileAccess.GenericRead | EFileAccess.GenericWrite
                    , FileShare.ReadWrite
                    , IntPtr.Zero
                    , FileMode.Open
                    , FileAttributes.Normal
                    , IntPtr.Zero
                    );
                if (handle == InvalidHandleValue)
                    throw new Exception();
                return handle;
            }

            public static IntPtr GetConsoleStandardOutput()
            {
                var handle = CreateFile
                    ("CONOUT$"
                    , EFileAccess.GenericWrite | EFileAccess.GenericRead
                    , FileShare.ReadWrite
                    , IntPtr.Zero
                    , FileMode.Open
                    , FileAttributes.Normal
                    , IntPtr.Zero
                    );
                if (handle == InvalidHandleValue)
                    throw new Exception();
                return handle;
            }
        }

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hdl);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFile
            (string lpFileName
            , [MarshalAs(UnmanagedType.U4)] EFileAccess dwDesiredAccess
            , [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode
            , IntPtr lpSecurityAttributes
            , [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition
            , [MarshalAs(UnmanagedType.U4)] FileAttributes dwFlagsAndAttributes
            , IntPtr hTemplateFile
            );

        public static readonly IntPtr InvalidHandleValue = new IntPtr(-1);
    }
}