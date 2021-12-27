using AntDesign;

namespace Minimal.Auth {
    public class StaticHelper {
        private static readonly Dictionary<int, ColLayoutParam> layouts = new Dictionary<int, ColLayoutParam>();
        public static ColLayoutParam GetColLayoutParam(int size) {
            if (!layouts.ContainsKey(size))
                layouts.Add(size, new ColLayoutParam() { Span = size });
            return layouts[size];
        }

        public static ListGridType Gutter = new ListGridType { Gutter = 16, Column = 4 };
        public static ListGridType GutterX = new ListGridType {
            Gutter = 16,
            Xs = 1,
            Sm = 2,
            Md = 4,
            Lg = 4,
            Xl = 6,
            Xxl = 3,
            Column = 3
        };

    }
}
