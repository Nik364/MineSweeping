using System;
using System.Collections.Generic;
using System.Linq;

namespace Nik.MineSweeping.Models
{
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
        public MineCell[] map;

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

            // -1 最初点击的位置
            if (mineNum > width * height - 1)
            {
                throw new ArgumentException("地雷数量超出可用空间数");
            }
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        public Mine Start()
        {
            if (isPalying)
            {
                return this;
            }

            this.isFisrtClick = true;
            this.isPalying = true;
            InitMap();
            return this;
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
            // 随机给每个单元格分配一个权重值，排序，然后取权重值最小的前地雷数个单元格布雷（不包含忽略区域）
            var ingoreIndex = this.CalcIndex(ignoreCell);
            var random = new Random();
            var mineNum = this.MineNum;
            var randNums = new List<Tuple<int, double>>();

            for (int i = this.Width * this.Height - 1; i >= 0; i--)
            {
                randNums.Add(new Tuple<int, double>(i, random.NextDouble()));
            }

            // 权重值排序
            randNums = (from items in randNums orderby items.Item2 select items).ToList();

            // 布雷
            foreach (var item in randNums)
            {
                if (item.Item1 == ingoreIndex)
                {
                    continue;
                }
                map[item.Item1].Status = CellStatus.Mine;
                if (--mineNum <= 0)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cell">点击的单元格</param>
        /// <returns>受影响的单元格</returns>
        public List<MineCell> Click(UserAction action, MineCell cell)
        {
            if (isFisrtClick)
            {
                // 初次点击布雷
                this.LayMines(cell);
                this.isFisrtClick = false;
            }

            // 循迹
            List<MineCell> list;
            if (action == UserAction.LeftClick)
            {
                list = this.Trailing(cell);
            }
            else
            {
                list = this.SetMark(cell);
            }

            // 判断游戏是否完成
            return list;
        }

        /// <summary>
        /// 设置标记
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private List<MineCell> SetMark(MineCell cell)
        {
            var list = new List<MineCell>();
            return list;
        }

        /// <summary>
        /// 寻迹
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private List<MineCell> Trailing(MineCell cell)
        {
            // BFS 搜索
            var open = new Queue<MineCell>();
            var closed = new List<MineCell>();
            var num = 0;

            // 1、判断自己是否为地雷
            if (this.map[this.CalcIndex(cell)].Status == CellStatus.Mine)
            {
                return closed;
            }
            // 2、判断周边是否存在地雷

            return closed;
        }

        /// <summary>
        /// 检查地图状态  游戏是否完成
        /// </summary>
        /// <returns></returns>
        private bool CheckStatus()
        {
            return true;
        }

        /// <summary>
        /// 根据地图坐标计算索引
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private int CalcIndex(MineCell cell)
        {
            return cell.X + cell.Y * this.Width;
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
        /// 地雷爆炸
        /// </summary>
        MineExploded = -11,

        /// <summary>
        /// 未知标记
        /// </summary>
        Marker = -18,

        /// <summary>
        /// 地雷标记
        /// </summary>
        MineMarker = -19,

        /// <summary>
        /// 地雷
        /// </summary>
        Mine = -999
    }

    /// <summary>
    /// 玩家动作
    /// </summary>
    public enum UserAction
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