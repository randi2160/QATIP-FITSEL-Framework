// Copyright � 2009 Syterra Software Inc.
// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License version 2.
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more details.

using System.Collections;

namespace fit.Test.Acceptance {
    public class NestedLists: AbstractNestedLists {
        public Fruit[] nested() {
            return new Fruit[]{ apple(), orange(), pear() };
        }
        private static Fruit apple() {
            IList list = new ArrayList();
            list.Add(new Element("a",1,firstInnerMostList()));
            list.Add(new Element("b",2,secondInnerMostList()));
            return new Fruit("apple",list);
        }
        private static IList firstInnerMostList() {
            IList innerList = new ArrayList();
            innerList.Add(new SubElement("x","y"));
            innerList.Add(new SubElement("x","z"));
            return innerList;
        }
        private static IList secondInnerMostList() {
            IList innerList = new ArrayList();
            innerList.Add(new SubElement("xx","ZZ"));
            return innerList;
        }
        private static Fruit orange() {
            IList list = new ArrayList();
            list.Add(new Element("c",3));
            return new Fruit("orange", list);
        }
        private static Fruit pear() {
            return new Fruit("pear", new ArrayList());
        }
    }
}