// reemplaza o añade en el pipeline antes de UseRouting
app.Use(async (context, next) =>
{
    if (!context.Request.IsHttps)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("HTTPS requerido.");
        return;
    }
    await next();
});