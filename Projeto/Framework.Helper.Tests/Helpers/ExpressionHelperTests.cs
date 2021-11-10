using Framework.Helper.Helpers;
using Xunit;

namespace Framework.Helper.Tests.Helpers
{
    public class ExpressionHelperTests
    {
        [Fact]
        public void DeveRetornarNomeDosAtributos()
        {
            Assert.Equal("Id", ExpressionHelper.GetPropertyName<Teste>(c => c.Id));
            Assert.Equal("Nome", ExpressionHelper.GetPropertyName<Teste>(c => c.Nome));
        }
    }

    public class Teste
    {
        public string Id { get; set; }
        public string Nome { get; set; }
    }
}
