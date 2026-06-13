using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.Services
{
    public interface IAvoidDuplicateCrossCuttingConcerns
    {
        List<string> AppliedCrossCuttingConcerns { get; }
    }
}
