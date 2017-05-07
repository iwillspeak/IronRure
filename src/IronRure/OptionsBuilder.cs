using System;

namespace IronRure
{
    public static class OptionsBuilder
    {
        public static Options WithSize(this Options opts, uint size)
        {
            opts.Size = size;
            return opts;
        }

        public static Options WithDfaSize(this Options opts, uint size)
        {
            opts.DfaSize = size;
            return opts;
        }
    }
}
