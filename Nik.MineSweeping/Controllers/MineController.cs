using System.Collections.Generic;
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
        /// </summary>
        private MineCell[] map;

        /// <summary>
        /// 是否初次点击
        /// </summary>
        bool isFisrtClick;

        /// <summary>
        /// 游戏是否正在进行
        /// </summary>
        bool isPalying = false;

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
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void Start()
        {
            if (isPalying)
            {
                return;
            }

            this.isFisrtClick = true;
            this.isPalying = true;
            InitMap();
        }

        /// <summary>
        /// 初始化游戏地图，布雷
        /// </summary>
        private void InitMap()
        {
            this.map = new MineCell[Width * Height];
            // 1、初始化地图为未知状态
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    // 按行列数据初始化
                    map[i * Width + j] = new MineCell
                    {
                        X = i,
                        Y = j,
                        Status = CellStatus.Unknown
                    };
                }
            }
        }

        /// <summary>
        /// 布雷
        /// </summary>
        /// <param name="ignoreCell">布雷忽略区域</param>
        private void LayMines(MineCell ignoreCell)
        {
            // 2、应在首次点击后，再布雷，这样就可避免初次点到雷

            // 3、布雷 
        }

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cell">点击的单元格</param>
        /// <returns>受影响的单元格</returns>
        public List<MineCell> Click(Action action, MineCell cell)
        {
            var list = new List<MineCell>();
            if (isFisrtClick)
            {
                this.LayMines(cell);
            }
            return list;
        }

        /// <summary>
        /// 检查地图状态  游戏是否完成
        /// </summary>
        /// <returns></returns>
        private bool CheckStatus()
        {
            return true;
        }
    }

    /// <summary>
    /// 地图单元格
    /// </summary>
    public class MineCell
    {
        /// <summary>
        /// 横坐标
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// 纵坐标 
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// 单元格状态
        /// </summary>
        public CellStatus Status { get; set; }
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

    /// <summary>
    /// 玩家动作
    /// </summary>
    public enum Action
    {
        /// <summary>
        /// 点击左键
        /// </summary>
        LeftClick = 0,

        /// <summary>
        /// 点击右键
        /// </summary>
        RightClick = 1
    }
}