using Microsoft.AspNetCore.Hosting;

namespace EducationPortalApp.Business.Helpers.UploadHelpers.Course
{
    public class CoursePictureDeleteHelper
    {
        public static void Delete(IHostingEnvironment hostingEnvironment, string file)
        {
            try
            {
                int index = file.IndexOf("CourseImages/");
                string relativeFileName = index != -1 ? file.Substring(index + "CourseImages/".Length) : file;
                string path = Path.Combine(hostingEnvironment.WebRootPath, "CourseImages", relativeFileName);
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
