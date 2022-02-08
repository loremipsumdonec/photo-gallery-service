using Microsoft.AspNetCore.Mvc.ModelBinding;
using PhotoGalleryService.Features.Magick.Instructions;
using PhotoGalleryService.Features.Magick.Services;

namespace PhotoGalleryService.Features.Magick.Bindings
{
    public class InstructionsModelBinder
        : IModelBinder
    {
        private readonly IInstructionFactory _factory;

        public InstructionsModelBinder(IInstructionFactory factory) 
        {
            _factory = factory;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(IEnumerable<IInstruction>))
            {
                return Task.CompletedTask;
            }

            var instructions = _factory.Create(
                bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue
            );

            bindingContext.Result = ModelBindingResult.Success(instructions);


            return Task.CompletedTask;
        }
    }
}
