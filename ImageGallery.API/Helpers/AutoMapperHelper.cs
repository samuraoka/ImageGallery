namespace ImageGallery.API.Helpers
{
    public class AutoMapperHelper
    {
        public static void InitializeAutoMapper()
        {
            // map to model
            // AutoMapper
            // https://www.nuget.org/packages/AutoMapper
            // Install-Package -Id AutoMapper -Project ImageGallery.API
            AutoMapper.Mapper.Initialize(cfg =>
            {
                // Map from Image(entity) to Image(model), and back
                cfg.CreateMap<Entities.Image, Model.Image>().ReverseMap();

                // TODO do another setting
            });

            AutoMapper.Mapper.AssertConfigurationIsValid();
        }
    }
}
