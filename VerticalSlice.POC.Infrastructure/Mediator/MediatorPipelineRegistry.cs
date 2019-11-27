using MediatR;
using Microsoft.EntityFrameworkCore;
using StructureMap;
using VerticalSlice.POC.DataAccess;
using VerticalSlice.POC.Services;

namespace VerticalSlice.POC.Infrastructure.Mediator
{
    public class MediatorPipelineRegistry : Registry
    {
        public MediatorPipelineRegistry()
        {
            Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.AssemblyContainingType(typeof(BaseHandler<,>));
                scanner.AssemblyContainingType(typeof(IRequestHandler<,>));

                scanner.ConnectImplementationsToTypesClosing(typeof(FluentValidation.IValidator<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(BaseValidator<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
            });


            For<ServiceFactory>().Use<ServiceFactory>(ctx => ctx.GetInstance);
            For<IMediator>().Use<MediatR.Mediator>();
            For<DbContext>().Use<VerticalSliceDbContext>();

            // Configure decorators over feature handlers
            For(typeof(IRequestHandler<,>)).DecorateAllWith(typeof(MediatorPipelineHandler<,>));
        }
    }
}
