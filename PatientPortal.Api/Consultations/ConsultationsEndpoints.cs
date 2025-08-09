using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using PatientPortal.Api.Common.Endpoints;
using PatientPortal.Application.Consultations.Dtos;
using PatientPortal.Application.Consultations.Services.Interfaces;
using PatientPortal.Common.FileSystem;

namespace PatientPortal.Api.Consultations;

public static class ConsultationsEndpoints
{
    public static IEndpointRouteBuilder MapConsultationsEndpoints(
        this IEndpointRouteBuilder self)
    {
        var group = self.MapGroup("/api/consultations")
            .WithTags("Consultations");

        group.MapGet("/{id:guid}", async (
            Guid id,
            IConsultationService consultationService,
            CancellationToken cancellationToken) =>
            {
                var result = await consultationService.GetByIdAsync(
                    id, cancellationToken);

                return result.ToEndpointResult();
            })
            .Produces<ConsultationResponseDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("/all", async (
            IConsultationService consultationService,
            CancellationToken cancellationToken) =>
            {
                var result = await 
                    consultationService.GetAllAsync(cancellationToken);

                return result.ToEndpointResult();
            })
            .Produces<List<ConsultationResponseDto>>(StatusCodes.Status200OK);

        group.MapPost("/", async (
            ScheduleConsultationRequestDto request,
            IConsultationService consultationService,
            CancellationToken cancellationToken) =>
            {
                var result = await consultationService.ScheduleAsync(
                    request, cancellationToken);

                return result.ToCreatedEndpointResult(
                    value => $"/api/consultations/{value.Id}");
            })
            .Produces<ConsultationResponseDto>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);

        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdateConsultationDetailsRequestDto request,
            IConsultationService consultationService,
            CancellationToken cancellationToken) =>
            {
                var result = await consultationService.UpdateDetailsAsync(
                    id,
                    request,
                    cancellationToken);

                return result.ToEndpointResult();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapPut("/{id:guid}/cancel", async (
            Guid id,
            IConsultationService consultationService,
            CancellationToken cancellationToken) =>
            {
                var result = await consultationService.CancelAsync(
                    id,
                    cancellationToken);

                return result.ToEndpointResult();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        // skipping service, initialization services, etc., for demo purposes
        group.MapPost("/attachments/upload", async (
            IFormFile file,
            IConfiguration config,
            BlobServiceClient blobServiceClient) =>
        {
            // hardcoded for demo simplicity
            var containerName = "patientportalblob";
            var publicAccessType = PublicAccessType.Blob;

            var containerClient = blobServiceClient.GetBlobContainerClient(
                containerName);

            await containerClient.CreateIfNotExistsAsync(publicAccessType);

            var original = Path.GetFileNameWithoutExtension(file.FileName)
                .ReplaceInvalidFileNameChars();

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            var blobName = $"{original}_{DateTime.UtcNow:yyyyMMddHHmmss}{ext}";
            var blobClient = containerClient.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            var url = blobClient.Uri.ToString();
            return Results.Ok(new { url });
        })
        .DisableAntiforgery(); // for demo purposes only

        return self;
    }
}
