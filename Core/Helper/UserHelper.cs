namespace Core.Helper;

public static class UserHelper
{
    public static string UserPhotoOrDefault(string? photoName)
    {
        string defaultPhotoPath = "~/images/avatar-default-icon.png";

        if (string.IsNullOrEmpty(photoName)) return defaultPhotoPath;

        //return Path.Combine(OsHelper.GetPhotoFilesPath(), photoName);
        return Path.Combine("~/Resources/PhotoFiles", photoName);
    }
}
