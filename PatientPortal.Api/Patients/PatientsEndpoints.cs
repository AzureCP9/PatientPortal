using Microsoft.AspNetCore.Mvc;
using PatientPortal.Api.Common.Endpoints;
using PatientPortal.Application.Patients.Dtos;
using PatientPortal.Application.Patients.Services.Interfaces;

namespace PatientPortal.Api.Patients;

public static class PatientsEndpoints
{
    // TODO: isolate file per method, keep it simple for now
    public static IEndpointRouteBuilder MapPatientsEndpoints(
        this IEndpointRouteBuilder self)
    {
        var group = self.MapGroup("/api/patients")
            .WithTags("Patients");

        group.MapGet("/{id:guid}", async (
            Guid id,
            IPatientService patientService,
            CancellationToken cancellationToken) =>
            {
                var result = await patientService.GetByIdAsync(
                    id,
                    cancellationToken);

                return result.ToEndpointResult();
            })
            .Produces<PatientResponseDto>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        group.MapGet("/all", async (
            IPatientService patientService,
            CancellationToken cancellationToken) =>
            {
                var result = await patientService.GetAllAsync(cancellationToken);
                return result.ToEndpointResult();
            })
            .Produces<List<PatientResponseDto>>(StatusCodes.Status200OK);

        group.MapPost("/", async (
            CreatePatientRequestDto request,
            IPatientService patientService,
            CancellationToken cancellationToken) =>
            {
                var result = await patientService.CreateAsync(
                    request,
                    cancellationToken);

                return result.ToCreatedEndpointResult(
                    value => $"/api/patients/{value.Id}");
            })
            .Produces<PatientResponseDto>(StatusCodes.Status201Created)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status409Conflict);


        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdatePatientDetailsRequestDto request,
            IPatientService patientService,
            CancellationToken cancellationToken) =>
            {
                var result = await patientService.UpdateDetailsAsync(
                    id,
                    request,
                    cancellationToken);

                return result.ToEndpointResult();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status404NotFound);

        return self;
    }
}