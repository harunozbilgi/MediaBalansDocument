using ImageMagick;
using ImageMagick.Formats;

namespace MediaBalansDocument.Library.Helpers;

public class ImageHelper
{
    private static readonly string local_path = "wwwroot/";
    public static async Task<bool> ResizeAsync(int width, int height, string source_path)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), local_path + source_path);

        if (!File.Exists(path))
            return false;

        try
        {
            using (MagickImage image = new(path))
            {
                image.Resize(width, height);
                await image.WriteAsync(path);
            }

            return true;
        }
        catch (Exception ex)
        {

            throw new Exception($"Beklenmedik hata: {ex.Message}");
        }

    }
    public static async Task<bool> CropAsync(int width, int height, string source_path)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), local_path + source_path);

        if (!File.Exists(path))
            return false;

        try
        {
            using (MagickImage image = new(path))
            {
                var request_file_ration = (double)width / (double)height;

                var current_file_ration = (double)width / (double)height;

                if (request_file_ration > current_file_ration)
                {
                    var new_height = image.Width * height / width;
                    image.Crop(image.Width, new_height);
                }
                else
                {
                    var new_width = image.Height * width / height;
                    image.Crop(image.Width, new_width);
                }

                image.Resize(width, height);
                await image.WriteAsync(path);
            }
            return true;
        }
        catch (Exception ex)
        {

            throw new Exception($"Beklenmedik hata: {ex.Message}");
        }

    }
    public static async Task<bool> OptimizeAsync(string source_path)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), local_path + source_path);

        if (!File.Exists(path))
            return false;

        try
        {
            using (MagickImage image = new(path))
            {
                image.Settings.SetDefines(new JpegWriteDefines()
                {
                    SamplingFactor = JpegSamplingFactor.Ratio420
                });
                image.Strip();
                image.Quality = 85;
                image.Interlace = Interlace.Jpeg;
                image.ColorSpace = ColorSpace.sRGB;
                await image.WriteAsync(path);
            }
            return true;
        }
        catch (Exception ex)
        {

            throw new Exception($"Beklenmedik hata: {ex.Message}");
        }

    }
    public static async Task<bool> QualityAsync(int quality, string source_path)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), local_path + source_path);

        if (!File.Exists(path))
            return false;

        try
        {
            using (MagickImage image = new(path))
            {
                image.Quality = quality;

                await image.WriteAsync(path);
            }
            return true;
        }
        catch (Exception ex)
        {

            throw new Exception($"Beklenmedik hata: {ex.Message}");
        }
    }
    public static async Task<bool> WaterMarkAsync(string source_path, string text, string color)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), local_path + source_path);

        if (!File.Exists(path))
            return false;

        try
        {
            using (MagickImage image = new(path))
            {
                using MagickImage copyright = new("xc:none", image.Width, image.Height);

                image.Draw(new Drawables()
                    .FillColor(new MagickColor(color))
                    .Gravity(Gravity.Northeast)
                    .Rotation(0)
                    .Text(10, 10, text));
                image.Tile(copyright, CompositeOperator.Over);
                await image.WriteAsync(path);
            }
            return true;
        }
        catch (Exception ex)
        {

            throw new Exception($"Beklenmedik hata: {ex.Message}");
        }
    }

}
