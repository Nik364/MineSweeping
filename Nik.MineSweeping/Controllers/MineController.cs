using System.Linq;
using System.Web.Mvc;

namespace Nik.MineSweeping.Controllers
{
    /// <summary>
    /// https://github.com/xxx407410849/MinesSweeper
    /// </summary>
    public class MineController : Controller
    {
        // GET: Mine
        public ActionResult Index()
        {
            return View();
        }


    }

    public class Mine
    {
        /// <summary>
        /// 横轴数量
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// 纵轴数量
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// 地雷数量
        /// </summary>
        public int MineNum { get; set; }

        /// <summary>
        /// 游戏地图
        /// -999_未知标记 -10_地雷 -11_地雷爆炸 -19_地雷标记 -1_初始状态（未知）大于等于0的数_地雷数量
        /// </summary>
        private CellStatus[,] map;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="width">横轴数量</param>
        /// <param name="height">纵轴数量</param>
        /// <param name="mineNum">地雷数量</param>
        public Mine(int width, int height, int mineNum)
        {
            this.Width = width;
            this.Height = height;
            this.MineNum = mineNum;

            InitMap();
        }

        /// <summary>
        /// 初始化游戏地图，布雷
        /// </summary>
        private void InitMap()
        {
            this.map = new CellStatus[Width, Height];
            // 1、初始化地图为未知状态
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    map[i, j] = CellStatus.Unknown;
                }
            }

            // 1、应在首次点击后，再布雷，这样就可避免初次点到雷

            // 2、布雷
            Enumerable.rep
        }
    }

    /// <summary>
    /// 地图单元格状态
    /// </summary>
    public enum CellStatus : int
    {
        /// <summary>
        /// 地雷数量：4
        /// </summary>
        Mine4 = 4,

        /// <summary>
        /// 地雷数量：3
        /// </summary>
        Mine3 = 3,

        /// <summary>
        /// 地雷数量：2
        /// </summary>
        Mine2 = 2,

        /// <summary>
        /// 地雷数量：1
        /// </summary>
        Mine1 = 1,

        /// <summary>
        /// 地雷数量：0
        /// </summary>
        Mine0 = 0,

        /// <summary>
        /// 初始状态（未知）
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// 地雷
        /// </summary>
        Mine = -10,

        /// <summary>
        /// 地雷爆炸
        /// </summary>
        MineExploded = -11,

        /// <summary>
        /// 地雷标记
        /// </summary>
        MineMarker = -19,

        /// <summary>
        /// 未知标记 -10_ -11_ -19_ -1_大于等于0的数_地雷数量
        /// </summary>
        Marker = -999
    }
}