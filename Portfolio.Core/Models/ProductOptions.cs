namespace Portfolio.Core.Models
{
    public enum ProductFormat
    {
        StandardPrint,
        Postcard,
        WallArt,
        Mug,
        Calendar,
        Magnet,
        HangingCanvas,
        Puzzle
    }

    public enum PrintFinish
    {
        None, // For items like mugs or calendars where finish might not apply
        Matte,
        Lustre,
        Glossy,
        Metallic
    }
}