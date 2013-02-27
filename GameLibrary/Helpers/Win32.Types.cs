using System;
using System.Runtime.InteropServices;

namespace GameLibrary.Helpers
{
    public static partial class Win32
    {
        public static partial class ConsoleLibrary
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct COORD
            {
                public short X;
                public short Y;
            }

            public struct SMALL_RECT
            {
                public short Left;
                public short Top;
                public short Right;
                public short Bottom;
            }

            public struct CONSOLE_SCREEN_BUFFER_INFO
            {
                public COORD dwSize;
                public COORD dwCursorPosition;
                public short wAttributes;
                public SMALL_RECT srWindow;
                public COORD dwMaximumWindowSize;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CONSOLE_SCREEN_BUFFER_INFO_EX
            {
                public uint cbSize;
                public COORD dwSize;
                public COORD dwCursorPosition;
                public short wAttributes;
                public SMALL_RECT srWindow;
                public COORD dwMaximumWindowSize;

                public ushort wPopupAttributes;
                public bool bFullscreenSupported;

                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
                public COLORREF[] ColorTable;

                public static CONSOLE_SCREEN_BUFFER_INFO_EX Create()
                {
                    return new CONSOLE_SCREEN_BUFFER_INFO_EX { cbSize = 96 };
                }
            }

            //[StructLayout(LayoutKind.Sequential)]
            //struct COLORREF
            //{
            //    public byte R;
            //    public byte G;
            //    public byte B;
            //}

            [StructLayout(LayoutKind.Sequential)]
            public struct COLORREF
            {
                public uint ColorDWORD;

                public COLORREF(System.Drawing.Color color)
                {
                    ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
                }

                public System.Drawing.Color GetColor()
                {
                    return System.Drawing.Color.FromArgb((int)(0x000000FFU & ColorDWORD),
                       (int)(0x0000FF00U & ColorDWORD) >> 8, (int)(0x00FF0000U & ColorDWORD) >> 16);
                }

                public void SetColor(System.Drawing.Color color)
                {
                    ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CONSOLE_FONT_INFO
            {
                public int nFont;
                public COORD dwFontSize;
            }

            [StructLayout(LayoutKind.Sequential)]
            public unsafe struct CONSOLE_FONT_INFO_EX
            {
                public uint cbSize;
                public uint nFont;
                public COORD dwFontSize;
                public ushort FontFamily;
                public ushort FontWeight;
                private fixed char FaceName[(int)LF_FACESIZE];

                private const uint LF_FACESIZE = 32;
            }

            [StructLayout(LayoutKind.Explicit)]
            public struct INPUT_RECORD
            {
                [FieldOffset(0)]
                public ushort EventType;

                [FieldOffset(4)]
                public KEY_EVENT_RECORD KeyEvent;

                [FieldOffset(4)]
                public MOUSE_EVENT_RECORD MouseEvent;

                [FieldOffset(4)]
                public WINDOW_BUFFER_SIZE_RECORD WindowBufferSizeEvent;

                [FieldOffset(4)]
                public MENU_EVENT_RECORD MenuEvent;

                [FieldOffset(4)]
                public FOCUS_EVENT_RECORD FocusEvent;
            };

            [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
            public struct KEY_EVENT_RECORD
            {
                [FieldOffset(0), MarshalAs(UnmanagedType.Bool)]
                public bool bKeyDown;

                [FieldOffset(4), MarshalAs(UnmanagedType.U2)]
                public ushort wRepeatCount;

                [FieldOffset(6), MarshalAs(UnmanagedType.U2)]

                //public VirtualKeys wVirtualKeyCode;
                public ushort wVirtualKeyCode;

                [FieldOffset(8), MarshalAs(UnmanagedType.U2)]
                public ushort wVirtualScanCode;

                [FieldOffset(10)]
                public char UnicodeChar;

                [FieldOffset(12), MarshalAs(UnmanagedType.U4)]

                //public ControlKeyState dwControlKeyState;
                public uint dwControlKeyState;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MOUSE_EVENT_RECORD
            {
                public COORD dwMousePosition;
                public uint dwButtonState;
                public uint dwControlKeyState;
                public uint dwEventFlags;
            }

            public struct WINDOW_BUFFER_SIZE_RECORD
            {
                public COORD dwSize;

                public WINDOW_BUFFER_SIZE_RECORD(short x, short y)
                {
                    dwSize = new COORD();
                    dwSize.X = x;
                    dwSize.Y = y;
                }
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct MENU_EVENT_RECORD
            {
                public uint dwCommandId;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct FOCUS_EVENT_RECORD
            {
                public uint bSetFocus;
            }

            //CHAR_INFO struct, which was a union in the old days
            // so we want to use LayoutKind.Explicit to mimic it as closely
            // as we can
            [StructLayout(LayoutKind.Explicit)]
            public struct CHAR_INFO
            {
                [FieldOffset(0)]
                private char UnicodeChar;

                [FieldOffset(0)]
                private char AsciiChar;

                [FieldOffset(2)] //2 bytes seems to work properly
                private UInt16 Attributes;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CONSOLE_CURSOR_INFO
            {
                private uint Size;
                private bool Visible;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CONSOLE_HISTORY_INFO
            {
                private ushort cbSize;
                private ushort HistoryBufferSize;
                private ushort NumberOfHistoryBuffers;
                private uint dwFlags;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CONSOLE_SELECTION_INFO
            {
                private uint Flags;
                private COORD SelectionAnchor;
                private SMALL_RECT Selection;

                // Flags values:
                private const uint CONSOLE_MOUSE_DOWN = 0x0008; // Mouse is down

                private const uint CONSOLE_MOUSE_SELECTION = 0x0004; //Selecting with the mouse
                private const uint CONSOLE_NO_SELECTION = 0x0000; //No selection
                private const uint CONSOLE_SELECTION_IN_PROGRESS = 0x0001; //Selection has begun
                private const uint CONSOLE_SELECTION_NOT_EMPTY = 0x0002; //Selection rectangle is not empty
            }

            // Enumerated type for the control messages sent to the handler routine
            public enum CtrlTypes : uint
            {
                CTRL_C_EVENT = 0,
                CTRL_BREAK_EVENT,
                CTRL_CLOSE_EVENT,
                CTRL_LOGOFF_EVENT = 5,
                CTRL_SHUTDOWN_EVENT
            }
        }

        [Flags]
        public enum EFileAccess : uint
        {
            //
            // Standart Section
            //

            AccessSystemSecurity = 0x1000000,   // AccessSystemAcl access type
            MaximumAllowed = 0x2000000,     // MaximumAllowed access type

            Delete = 0x10000,
            ReadControl = 0x20000,
            WriteDAC = 0x40000,
            WriteOwner = 0x80000,
            Synchronize = 0x100000,

            StandardRightsRequired = 0xF0000,
            StandardRightsRead = ReadControl,
            StandardRightsWrite = ReadControl,
            StandardRightsExecute = ReadControl,
            StandardRightsAll = 0x1F0000,
            SpecificRightsAll = 0xFFFF,

            FILE_READ_DATA = 0x0001,        // file & pipe
            FILE_LIST_DIRECTORY = 0x0001,       // directory
            FILE_WRITE_DATA = 0x0002,       // file & pipe
            FILE_ADD_FILE = 0x0002,         // directory
            FILE_APPEND_DATA = 0x0004,      // file
            FILE_ADD_SUBDIRECTORY = 0x0004,     // directory
            FILE_CREATE_PIPE_INSTANCE = 0x0004, // named pipe
            FILE_READ_EA = 0x0008,          // file & directory
            FILE_WRITE_EA = 0x0010,         // file & directory
            FILE_EXECUTE = 0x0020,          // file
            FILE_TRAVERSE = 0x0020,         // directory
            FILE_DELETE_CHILD = 0x0040,     // directory
            FILE_READ_ATTRIBUTES = 0x0080,      // all
            FILE_WRITE_ATTRIBUTES = 0x0100,     // all

            //
            // Generic Section
            //

            GenericRead = 0x80000000,
            GenericWrite = 0x40000000,
            GenericExecute = 0x20000000,
            GenericAll = 0x10000000,

            SPECIFIC_RIGHTS_ALL = 0x00FFFF,

            FILE_ALL_ACCESS =
            StandardRightsRequired |
            Synchronize |
            0x1FF,

            FILE_GENERIC_READ =
            StandardRightsRead |
            FILE_READ_DATA |
            FILE_READ_ATTRIBUTES |
            FILE_READ_EA |
            Synchronize,

            FILE_GENERIC_WRITE =
            StandardRightsWrite |
            FILE_WRITE_DATA |
            FILE_WRITE_ATTRIBUTES |
            FILE_WRITE_EA |
            FILE_APPEND_DATA |
            Synchronize,

            FILE_GENERIC_EXECUTE =
            StandardRightsExecute |
              FILE_READ_ATTRIBUTES |
              FILE_EXECUTE |
              Synchronize
        }

        [Flags]
        public enum EFileShare : uint
        {
            /// <summary>
            ///
            /// </summary>
            None = 0x00000000,

            /// <summary>
            /// Enables subsequent open operations on an object to request read access.
            /// Otherwise, other processes cannot open the object if they request read access.
            /// If this flag is not specified, but the object has been opened for read access, the function fails.
            /// </summary>
            Read = 0x00000001,

            /// <summary>
            /// Enables subsequent open operations on an object to request write access.
            /// Otherwise, other processes cannot open the object if they request write access.
            /// If this flag is not specified, but the object has been opened for write access, the function fails.
            /// </summary>
            Write = 0x00000002,

            /// <summary>
            /// Enables subsequent open operations on an object to request delete access.
            /// Otherwise, other processes cannot open the object if they request delete access.
            /// If this flag is not specified, but the object has been opened for delete access, the function fails.
            /// </summary>
            Delete = 0x00000004
        }

        public enum ECreationDisposition : uint
        {
            /// <summary>
            /// Creates a new file. The function fails if a specified file exists.
            /// </summary>
            New = 1,

            /// <summary>
            /// Creates a new file, always.
            /// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes,
            /// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
            /// </summary>
            CreateAlways = 2,

            /// <summary>
            /// Opens a file. The function fails if the file does not exist.
            /// </summary>
            OpenExisting = 3,

            /// <summary>
            /// Opens a file, always.
            /// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
            /// </summary>
            OpenAlways = 4,

            /// <summary>
            /// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
            /// The calling process must open the file with the GENERIC_WRITE access right.
            /// </summary>
            TruncateExisting = 5
        }

        [Flags]
        public enum EFileAttributes : uint
        {
            Readonly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            Write_Through = 0x80000000,
            Overlapped = 0x40000000,
            NoBuffering = 0x20000000,
            RandomAccess = 0x10000000,
            SequentialScan = 0x08000000,
            DeleteOnClose = 0x04000000,
            BackupSemantics = 0x02000000,
            PosixSemantics = 0x01000000,
            OpenReparsePoint = 0x00200000,
            OpenNoRecall = 0x00100000,
            FirstPipeInstance = 0x00080000
        }
    }
}