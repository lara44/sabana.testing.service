namespace sabana.testing.service.tests.unit.presentation.common;

using Presentation.Common;

[TestClass]
public sealed class ResponseDtoTests
{
    [TestMethod]
    public void Given_NoArguments_When_CreateDefaultResponse_Then_UsesDefaultValues()
    {
        // Arrange (Given)

        // Act (When)
        var response = new ResponseDto<string>();

        // Assert (Then)
        Assert.AreEqual(200, response.Code);
        Assert.IsFalse(response.Error);
        Assert.AreEqual(string.Empty, response.Message);
        Assert.IsNull(response.Data);
    }

    [TestMethod]
    public void Given_InputValues_When_UseFactoryMethods_Then_ReturnExpectedResponses()
    {
        // Arrange (Given)
        var successData = "created";

        // Act (When)
        var success = ResponseDto<string>.Success("Producto creado correctamente.", successData);
        var genericError = ResponseDto<string>.ErrorGeneric(409, "Conflicto.", "payload");
        var badRequest = ResponseDto<string>.ErrorBadRequest("Solicitud invalida.", "payload");
        var notFound = ResponseDto<string>.ErrorNotFound("No encontrado.", "payload");
        var unauthorized = ResponseDto<string>.ErrorUnauthorized("No autorizado.", "payload");
        var forbidden = ResponseDto<string>.ErrorForbidden("Prohibido.", "payload");
        var serverError = ResponseDto<string>.ErrorInternalServerError("Error interno.", "payload");

        // Assert (Then)
        Assert.AreEqual(200, success.Code);
        Assert.IsFalse(success.Error);
        Assert.AreEqual("Producto creado correctamente.", success.Message);
        Assert.AreEqual(successData, success.Data);

        Assert.AreEqual(409, genericError.Code);
        Assert.IsTrue(genericError.Error);
        Assert.AreEqual("Conflicto.", genericError.Message);
        Assert.AreEqual("payload", genericError.Data);

        Assert.AreEqual(400, badRequest.Code);
        Assert.IsTrue(badRequest.Error);
        Assert.AreEqual("Solicitud invalida.", badRequest.Message);
        Assert.AreEqual("payload", badRequest.Data);

        Assert.AreEqual(404, notFound.Code);
        Assert.IsTrue(notFound.Error);
        Assert.AreEqual("No encontrado.", notFound.Message);
        Assert.AreEqual("payload", notFound.Data);

        Assert.AreEqual(401, unauthorized.Code);
        Assert.IsTrue(unauthorized.Error);
        Assert.AreEqual("No autorizado.", unauthorized.Message);
        Assert.AreEqual("payload", unauthorized.Data);

        Assert.AreEqual(403, forbidden.Code);
        Assert.IsTrue(forbidden.Error);
        Assert.AreEqual("Prohibido.", forbidden.Message);
        Assert.AreEqual("payload", forbidden.Data);

        Assert.AreEqual(500, serverError.Code);
        Assert.IsTrue(serverError.Error);
        Assert.AreEqual("Error interno.", serverError.Message);
        Assert.AreEqual("payload", serverError.Data);
    }
}
