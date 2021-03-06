﻿using System;
using System.Collections.Generic;
using System.Text;
using DFC.Personalisation.Common.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DFC.Personalisation.Common.UnitTests.Helpers
{
    class HyphenControllerTrasnformerTests
    {
        [Test]
        public void WhenHyphenatedRoute_ThenAddCleanHyphenatedRouteToApplication()
        {
            var sut = new HyphenControllerTransformer();
            var result = sut.TransformOutbound("YourDetails");
            result.Should().Be("your-details");
        }

        [Test] public void WhenHyphenatedRouteNull_ThenReturnNull()
        {
            var sut = new HyphenControllerTransformer();
            var result = sut.TransformOutbound(null);
            result.Should().BeNull();
        }
    }
}
