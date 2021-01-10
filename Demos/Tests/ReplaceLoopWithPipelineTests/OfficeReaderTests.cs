using FluentAssertions;
using ReplaceLoopWithPipeline;
using System;
using Xunit;

namespace ReplaceLoopWithPipelineTests
{
    public class OfficeReaderTests
    {
        [Fact]
        public void GetNorwayOffices_empty_input_should_be_empty()
        {
            var sut = new OfficeReader();

            var result = sut.GetNorwayOffices(string.Empty);

            result.Should().BeEmpty();
        }

        [Fact]
        public void GetNorwayOffices_first_line_should_be_skipped()
        {
            const string invalidFirstLine = "this is an invalid line";
            var sut = new OfficeReader();

            var result = sut.GetNorwayOffices(invalidFirstLine);

            result.Should().BeEmpty();
        }


        [Fact]
        public void GetNorwayOffices_line_without_country_should_throw_indexoutofrange()
        {
            const string invalidFirstLine = "\nthis is an invalid line without country";
            var sut = new OfficeReader();

            Action act = () => sut.GetNorwayOffices(invalidFirstLine);

            act.Should().Throw<IndexOutOfRangeException>();
        }

        [Fact]
        public void GetNorwayOffices_empty_lines_should_be_skipped()
        {
            const string inputWithTwoOfficesAndEmptyLine = "\nx, Norway, , y\n \n z, Norway, , \n";
            const int expectedCount = 2;

            var sut = new OfficeReader();

            var result = sut.GetNorwayOffices(inputWithTwoOfficesAndEmptyLine);

            result.Should().HaveCount(expectedCount);
        }

        [Fact]
        public void GetNorwayOffices_without_norway_lines_should_be_empty()
        {
            const string inputWithoutNorway = "\nx, Belgium, , y\nz, Netherlands, , \n";

            var sut = new OfficeReader();

            var result = sut.GetNorwayOffices(inputWithoutNorway);

            result.Should().BeEmpty();
        }

        [Fact]
        public void GetNorwayOffices_Norway_office_should_be_returned()
        {
            const string inputWithNorwayOffice = "\ncity, Norway, phone, address";
            var expected = new Office { City = "city", Phone = "phone", Address = "address" };

            var sut = new OfficeReader();

            var result = sut.GetNorwayOffices(inputWithNorwayOffice);

            result.Should().OnlyContain(office => office.Equals(expected));
        }

    }
}
