using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace ClientAppGiaBuild.Service
{
    public enum GraphMode
    {
        Follow, AutoScale, Hold
    }
    public class Graph
    {
        public List<RollingPointPairList> CurveData = new List<RollingPointPairList>();
        public void Initailize(ZedGraphControl graph, double xMin, double xMax, double yMin, double yMax, string title, string xTitle, string yTitle)
        {
            GraphPane pane = graph.GraphPane;
            pane.Title.Text = title;
            pane.XAxis.Title.Text = xTitle;
            pane.YAxis.Title.Text = yTitle;
            pane.XAxis.Scale.Max = xMax;
            pane.XAxis.Scale.Min = xMin;
            pane.YAxis.Scale.Max = yMax;
            pane.YAxis.Scale.Min = yMin;
            pane.AxisChange();
            graph.Invalidate();
            graph.Refresh();
        }
        public void AddGraph(ZedGraphControl graph, string name, System.Drawing.Color color, SymbolType symbol = SymbolType.None)
        {
            GraphPane pane = graph.GraphPane;
            CurveData.Add(new RollingPointPairList(6000));
            pane.AddCurve(name, CurveData[CurveData.Count - 1], color, symbol);
            graph.Invalidate();
            graph.Refresh();
        }
        public void UpdateGraph(ZedGraphControl graph, int index, double x, double y, GraphMode mode, double paneWidth = 5.0)
        {
            GraphPane pane = graph.GraphPane;
            if (pane.CurveList.Count <= index)
                return;
            LineItem line = pane.CurveList[index] as LineItem;
            if (line == null)
                return;
            IPointListEdit point_list = line.Points as IPointListEdit;
            point_list.Add(x, y);
            Scale xScale = pane.XAxis.Scale;
            Scale yScale = pane.YAxis.Scale;
            switch (mode)
            {
                case GraphMode.Follow:
                    if (x > xScale.Max - xScale.MajorStep)
                    {
                        xScale.Max = x + xScale.MajorStep;
                        xScale.Min = xScale.Max - paneWidth;
                    }
                    if (y < yScale.Min + yScale.MajorStep)
                    {
                        yScale.Min = y - yScale.MajorStep;
                    }
                    if (y > yScale.Max - yScale.MajorStep)
                    {
                        yScale.Max = y + yScale.MajorStep;
                    }
                    DrawGraph(graph);
                    break;
                case GraphMode.AutoScale:
                    if (x < xScale.Min + xScale.MajorStep)
                    {
                        xScale.Min = x - xScale.MajorStep;
                    }
                    if (x > xScale.Max - xScale.MajorStep)
                    {
                        xScale.Max = x + xScale.MajorStep;
                    }
                    if (y < yScale.Min + yScale.MajorStep)
                    {
                        yScale.Min = y - yScale.MajorStep;
                    }
                    if (y > yScale.Max - yScale.MajorStep)
                    {
                        yScale.Max = y + yScale.MajorStep;
                    }
                    DrawGraph(graph);
                    break;
                case GraphMode.Hold:
                    DrawGraph(graph);
                    break;
            }
        }
        public void DrawGraph(ZedGraphControl graph)
        {
            graph.Invalidate();
            graph.Refresh();
            graph.AxisChange();
        }
        public void ClearGraph(ZedGraphControl graph)
        {
            graph.GraphPane.CurveList.Clear();
            graph.GraphPane.GraphObjList.Clear();
            CurveData.Clear();
        }
    }
}
