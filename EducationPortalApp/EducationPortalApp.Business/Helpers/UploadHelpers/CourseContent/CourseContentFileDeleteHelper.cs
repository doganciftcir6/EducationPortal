using Microsoft.AspNetCore.Hosting;

namespace EducationPortalApp.Business.Helpers.UploadHelpers.CourseContent
{
    public class CourseContentFileDeleteHelper
    {
        public static void Delete(IHostingEnvironment hostingEnvironment, string file)
        {
            try
            {
                int index = file.IndexOf("CourseContentFiles/");
                string relativeFileName = index != -1 ? file.Substring(index + "CourseContentFiles/".Length) : file;
                string path = Path.Combine(hostingEnvironment.WebRootPath, "CourseContentFiles", relativeFileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
