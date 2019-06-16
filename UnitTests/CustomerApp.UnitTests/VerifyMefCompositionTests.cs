namespace CustomerApp.UnitTests
{
    using System;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using Controllers.APIv1;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class VerifyMefCompositionTests
    {
        [Test]
        public void VerifyContracts()
        {
            var container = MefContainer.ConfigureContainer(true);

            AssertCanCompose<CustomersController>(container);
            AssertCanCompose<NotesController>(container);
        }

        private static void AssertCanCompose<TType>(ExportProvider container, int howMany = 1)
        {
            Action sut = () =>
            {
                var exports = container.GetExportedValues<TType>().ToList();

                if (exports.Count() == howMany) return;

                var exportNames = string.Join(",", exports.Select(e => e.GetType().Name));
                throw new CompositionException($"Expected {howMany} exports, but found {exports.Count}: {exportNames}");
            };

            sut.Should().NotThrow();
        }
    }
}