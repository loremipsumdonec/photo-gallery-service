using PhotoGalleryService.Features.Gallery.Commands;
using PhotoGalleryService.Features.Gallery.Models;
using PhotoGalleryService.Features.Gallery.Queries;
using Boilerplate.Features.Core.Commands;
using Boilerplate.Features.Core.Queries;

namespace PhotoGalleryService.Features.Gallery.Schema
{
    public class GalleryMutation
    {
        public async Task<Album> CreateTemplate(
            string name,
            string? description,
            [Service] ICommandDispatcher dispatcher,
            [Service] IQueryDispatcher queryDispatcher)
        {

            var command = new CreateAlbum(name, description);

            if (await dispatcher.DispatchAsync(command))
            {
                string templateId = (string)command.CommandResult.Output;
                return await queryDispatcher.DispatchAsync<Album>(new GetAlbum(templateId));
            }

            throw command.CommandResult.Exception;
        }

        public async Task<Album> UpdateTemplate(
            string templateId,
            string name,
            string? description,
            [Service] ICommandDispatcher dispatcher,
            [Service] IQueryDispatcher queryDispatcher) 
        {

            var command = new UpdateAlbum(templateId, name, description);

            if (await dispatcher.DispatchAsync(command))
            {
                return await queryDispatcher.DispatchAsync<Album>(new GetAlbum(templateId));
            }

            throw command.CommandResult.Exception;
        }

        public async Task<Album> DeleteTemplate(
            string templateId,
            [Service] ICommandDispatcher dispatcher,
            [Service] IQueryDispatcher queryDispatcher)
        {
            var command = new DeleteAlbum(templateId);

            if (await dispatcher.DispatchAsync(command))
            {
                return await queryDispatcher.DispatchAsync<Album>(new GetAlbum(templateId));
            }

            throw command.CommandResult.Exception;
        }
    }
}
