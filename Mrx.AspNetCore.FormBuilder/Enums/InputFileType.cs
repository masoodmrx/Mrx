namespace Mrx.AspNetCore.FormBuilder
{
    public enum InputFileType
    {
        File,
        [Data(FileType.Image)]
        Image,
        [Data(FileType.Video)]
        Video,
        [Data(FileType.Audio)]
        Audio
    }
}
