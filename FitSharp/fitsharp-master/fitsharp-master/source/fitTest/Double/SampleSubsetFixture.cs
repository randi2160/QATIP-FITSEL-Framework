// Copyright � 2012 Syterra Software Inc.
// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License version 2.
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

using fitlibrary;

namespace fit.Test.Double {
    public class SampleSubsetFixture: SubsetFixture {
        public SampleSubsetFixture(): base(SetUpSampleArrayFixture.List) {}
    }
}
