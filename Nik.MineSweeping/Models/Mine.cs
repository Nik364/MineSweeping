using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public MineCell[] Map { get; private set; }

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
            this.Map = new MineCell[Width * Height];
            // 1、初始化地图为未知状态
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    // 按行列数据初始化
                    Map[y * Width + x] = new MineCell
                    {
                        X = x,
                        Y = y,
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
            var ingoreIndex = this.CalcIndex(ignoreCell.X, ignoreCell.Y);
            var random = new Random(DateTime.Now.Millisecond);
            var mineNum = this.MineNum;
            var randNums = new List<Tuple<int, double>>();

            for (int i = this.Map.Length - 1; i >= 0; i--)
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
                Map[item.Item1].IsMine = true;
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



            for (int i = 0; i < this.Map.Length; i++)
            {
                if (i % this.Width == 0)
                {
                    Debug.WriteLine("");
                }
                Debug.Write($"{this.Map[i].Status}\t");
            }
            
            List<MineCell> list;
            if (action == UserAction.LeftClick)
            {
                // 寻迹
                list = this.Trailing(cell);
                if (list.Count <= 0)
                {
                    int i = 0;
                }
            }
            else
            {
                // 设置标识
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
            var curOpen = new Queue<MineCell>();
            var closed = new List<MineCell>();
            var num = 0;

            var curCell = this.FindCell(cell.X, cell.Y);


            // 1、判断自己是否为地雷
            if (curCell.IsMine)
            {
                return closed;
            }

            // 2、开始检索
            // 当前对象加入检索范围
            open.Enqueue(curCell);
            while (open.Count > 0)
            {
                num = 0;
                curOpen = new Queue<MineCell>();
                curCell = open.Dequeue();

                // 2.1、判断周边是否存在地雷
                for (int yStep = -1; yStep <= 1; yStep++)
                {
                    for (int xStep = -1; xStep <= 1; xStep++)
                    {
                        if ((xStep == 0 && yStep == 0)
                            || curCell.X + xStep < 0
                            || curCell.X + xStep >= this.Width
                            || curCell.Y + yStep < 0
                            || curCell.Y + yStep >= this.Height)
                        {
                            continue;
                        }

                        var tempCell = this.FindCell(curCell.X + xStep, curCell.Y + yStep);

                        if (closed.Contains(tempCell))
                        {
                            // 已检查
                            continue;
                        }

                        if (tempCell.IsMine)
                        {
                            num++;
                        }
                        else if (tempCell.Status == CellStatus.Unknown)
                        {
                            curOpen.Enqueue(tempCell);
                        }
                        // Console.Write("{0}-{1}:{2} ", tempCell.X, tempCell.Y, num);
                    }
                }
                curCell.Status = (CellStatus)num;
                closed.Add(curCell);

                if (num == 0)
                {
                    while (curOpen.Count > 0)
                    {
                        if (!open.Contains(curOpen.Peek()))
                        {
                            open.Enqueue(curOpen.Dequeue());
                        }
                        else
                        {
                            curOpen.Dequeue();
                        }
                    }
                }
                Debug.WriteLine("\t{0}-{1}:{2}", curCell.X, curCell.Y, num);
            }
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
        /// 根据坐标查询单元格
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private MineCell FindCell(int x, int y)
        {
            return this.Map[this.CalcIndex(x, y)];
        }

        /// <summary>
        /// 根据地图坐标计算索引
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int CalcIndex(int x, int y)
        {
            return x + y * this.Width;
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
        /// 单元格状态（UI）
        /// </summary>
        public CellStatus Status { get; set; }

        /// <summary>
        /// 是否地雷（数据）
        /// </summary>
        public bool IsMine { get; set; }
    }

    /// <summary>
    /// 地图单元格状态
    /// </summary>
    public enum CellStatus : int
    {
        /// <summary>
        /// 地雷数量：8
        /// </summary>
        Mine8 = 8,

        /// <summary>
        /// 地雷数量：7
        /// </summary>
        Mine7 = 7,

        /// <summary>
        /// 地雷数量：6
        /// </summary>
        Mine6 = 6,
        /// <summary>
        /// 地雷数量：5
        /// </summary>
        Mine5 = 5,

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
        MineMarker = -19
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