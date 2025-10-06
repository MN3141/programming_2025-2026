// LOW_COMPAT Macro
// When defined, the FastImage class is optimized to handle specific 24bpp .jpeg files (such as lincoln, tiger, wolf).
// Removing this macro enables support for 32bpp (4 bytes per pixel) image formats, like .png, which include an alpha channel for transparency,
// but will cause a minor performance loss.
// To disable LOW_COMPAT mode, simply comment the line below and re-compile the program.
#define LOW_COMPAT 

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ComputerVision
{
    /// <summary>
    /// The FastImage class provides fast pixel manipulation for 24bpp and 32bpp images.
    /// The class allows locking the image into memory and accessing pixels directly 
    /// for performance improvements in image processing tasks.
    /// </summary>
    public sealed class FastImage : IDisposable
    {
        // Public properties for image dimensions
        public int Height { get; private set; }
        public int Width { get; private set; }

        private Bitmap image; 
        private Rectangle rectangle; 
        private BitmapData bitmapData; 
        
        private int bytesPerPixel;
        private bool isLocked = false;
        private IntPtr basePointer; // Pointer to the first byte of the pixel data
        private int stride; // The stride (width in bytes) of a single row of pixels

        private bool disposed = false;

        // Structs for pixel data storage in memory (24bpp and 32bpp formats)
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct PixelData24
        {
            public byte blue, green, red; 
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct PixelData32
        {
            public byte blue, green, red, alpha; 
        }

        /// <summary>
        /// Constructor for the FastImage class. Initializes the FastImage object with a provided Bitmap.
        /// Throws exceptions for null bitmap or unsupported pixel formats.
        /// </summary>
        /// <param name="bitmap">The Bitmap object to be used for fast pixel operations</param>
        public FastImage(Bitmap bitmap)
        {
            image = bitmap ?? throw new ArgumentNullException(nameof(bitmap));
            Width = image.Width;
            Height = image.Height;

            // Check if LOW_COMPAT is defined for 24bpp image handling
#if LOW_COMPAT
            // Only 24bpp images are supported when LOW_COMPAT is defined
            if (image.PixelFormat == PixelFormat.Format24bppRgb)
            {
                bytesPerPixel = 3; // 3 bytes per pixel (R, G, B)
            }
            else
            {
                throw new NotSupportedException("Only 24bpp images are supported when LOW_COMPAT is enabled. Check FastImage.cs for more information");
            }
#else
            // Support both 24bpp and 32bpp images when LOW_COMPAT is not defined
            if (image.PixelFormat == PixelFormat.Format24bppRgb)
            {
                bytesPerPixel = 3; // 3 bytes per pixel (R, G, B)
            }
            else if (image.PixelFormat == PixelFormat.Format32bppArgb || image.PixelFormat == PixelFormat.Format32bppRgb)
            {
                bytesPerPixel = 4; // 4 bytes per pixel (R, G, B, A)
            }
            else
            {
                throw new NotSupportedException($"Only 24bpp and 32bpp images are supported. Curent image is {image.PixelFormat}");
            }
#endif
        }


        /// <summary>
        /// Locks the image in memory for direct pixel manipulation.
        /// Throws an exception if the image is already locked.
        /// </summary>
        public void Lock()
        {
            if (isLocked)
                throw new InvalidOperationException("Bitmap is already locked.");

            try
            {
                rectangle = new Rectangle(0, 0, Width, Height); // Define the entire image area
                bitmapData = image.LockBits(rectangle, ImageLockMode.ReadWrite, image.PixelFormat);
                basePointer = bitmapData.Scan0; // Get the pointer to the first pixel data
                stride = bitmapData.Stride;
                isLocked = true;
            }
            catch
            {
                isLocked = false;
                throw;
            }
        }

        /// <summary>
        /// Unlocks the image and allows it to be released from memory.
        /// Throws an exception if the image is not locked.
        /// </summary>
        public void Unlock()
        {
            if (!isLocked)
                throw new InvalidOperationException("Bitmap is not locked.");

            try
            {
                image.UnlockBits(bitmapData);
            }
            finally
            {
                isLocked = false;
            }
        }

        /// <summary>
        /// Retrieves the color of a pixel at a specific column and row.
        /// Uses different implementations based on the LOW_COMPAT macro.
        /// If LOW_COMPAT is defined, it will only use 24bpp logic.
        /// </summary>
        /// <param name="col">The column of the pixel</param>
        /// <param name="row">The row of the pixel</param>
        /// <returns>The color of the pixel</returns>
        public Color GetPixel(int col, int row)
        {
#if LOW_COMPAT
            // 24bpp mode, independent of the actual image format
            unsafe
            {
                byte* pPixel = (byte*)basePointer + (row * stride) + (col * 3); // Pointer to the pixel data (24bpp)
                return Color.FromArgb(pPixel[2], pPixel[1], pPixel[0]);
            }
#else
            // General mode that handles both 24bpp and 32bpp formats
            unsafe
            {
                byte* pPixel = (byte*)basePointer + (row * stride) + (col * bytesPerPixel);

                if (bytesPerPixel == 3)
                {
                    PixelData24* pPixel24 = (PixelData24*)pPixel; // For 24bpp
                    return Color.FromArgb(pPixel24->red, pPixel24->green, pPixel24->blue);
                }
                else
                {
                    PixelData32* pPixel32 = (PixelData32*)pPixel; // For 32bpp
                    return Color.FromArgb(pPixel32->alpha, pPixel32->red, pPixel32->green, pPixel32->blue);
                }
            }
#endif
        }

        /// <summary>
        /// Sets the color of a pixel at a specific column and row.
        /// Uses different implementations based on the LOW_COMPAT macro.
        /// If LOW_COMPAT is defined, it will only use 24bpp logic.
        /// </summary>
        /// <param name="col">The column of the pixel</param>
        /// <param name="row">The row of the pixel</param>
        /// <param name="c">The color to set</param>
        public void SetPixel(int col, int row, Color c)
        {
#if LOW_COMPAT
            // 24bpp mode, independent of the actual image format
            unsafe
            {
                byte* pPixel = (byte*)basePointer + (row * stride) + (col * 3); // Pointer to the pixel data (24bpp)
                pPixel[2] = c.R;
                pPixel[1] = c.G;
                pPixel[0] = c.B;
            }
#else
            // General mode that handles both 24bpp and 32bpp formats
            unsafe
            {
                byte* pPixel = (byte*)basePointer + (row * stride) + (col * bytesPerPixel);

                if (bytesPerPixel == 3)
                {
                    PixelData24* pPixel24 = (PixelData24*)pPixel; // For 24bpp
                    pPixel24->red = c.R;
                    pPixel24->green = c.G;
                    pPixel24->blue = c.B;
                }
                else
                {
                    PixelData32* pPixel32 = (PixelData32*)pPixel; // For 32bpp
                    pPixel32->red = c.R;
                    pPixel32->green = c.G;
                    pPixel32->blue = c.B;
                    pPixel32->alpha = c.A;
                }
            }
#endif
        }


        /// <summary>
        /// Retrieves the color of a pixel from a 24bpp image at a specific column and row.
        /// </summary>
        public unsafe Color GetPixel24bpp(int col, int row)
        {
            byte* pPixel = (byte*)basePointer + (row * stride) + (col * 3); // Get pointer for 24bpp image
            return Color.FromArgb(pPixel[2], pPixel[1], pPixel[0]);
        }

        /// <summary>
        /// Retrieves the color of a pixel from a 32bpp image at a specific column and row.
        /// </summary>
        public unsafe Color GetPixel32bpp(int col, int row)
        {
            byte* pPixel = (byte*)basePointer + (row * stride) + (col * 4); // Get pointer for 32bpp image
            return Color.FromArgb(pPixel[3], pPixel[2], pPixel[1], pPixel[0]);
        }

        /// <summary>
        /// Sets the color of a pixel in a 24bpp image at a specific column and row.
        /// </summary>
        public unsafe void SetPixel24bpp(int col, int row, Color c)
        {
            byte* pPixel = (byte*)basePointer + (row * stride) + (col * 3); // Get pointer for 24bpp image
            pPixel[2] = c.R;
            pPixel[1] = c.G;
            pPixel[0] = c.B;
        }

        /// <summary>
        /// Sets the color of a pixel in a 32bpp image at a specific column and row.
        /// </summary>
        public unsafe void SetPixel32bpp(int col, int row, Color c)
        {
            byte* pPixel = (byte*)basePointer + (row * stride) + (col * 4); // Get pointer for 32bpp image
            pPixel[2] = c.R;
            pPixel[1] = c.G;
            pPixel[0] = c.B;
            pPixel[3] = c.A;
        }

        /// <summary>
        /// Returns the internal Bitmap object.
        /// </summary>
        /// <returns>The Bitmap object used for pixel operations.</returns>
        public Bitmap GetBitMap()
        {
            return image;
        }

        /// <summary>
        /// Disposes of the Bitmap resources used by the FastImage class.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Disposes of the Bitmap resources used by the FastImage class.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (image != null)
                    {
                        image.Dispose();
                        image = null;
                    }
                }
                disposed = true;
            }
        }
        /// <summary>
        /// Destructor of the FastImage class, ensures properly disposing of unmanaged resurces
        /// </summary>
        ~FastImage()
        {
            Dispose(false);
        }
    }
}
