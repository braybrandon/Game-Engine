// GameEngine.Tools/MainForm.cs
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GameEngine.Common; // <-- your TileMap lives here

namespace GameEngine.Tools;

public sealed class CanvasPanel : Panel
{
    public CanvasPanel()
    {
        // True double-buffering + no background erase (prevents flicker)
        this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                      ControlStyles.UserPaint |
                      ControlStyles.OptimizedDoubleBuffer, true);
        this.UpdateStyles();

        this.DoubleBuffered = true;
        this.ResizeRedraw = true;
    }

    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        // Intentionally skip default background painting to avoid flicker
        // base.OnPaintBackground(pevent);
    }
}

public class MainForm : Form
{
    // ---- Config ----
    private int _tileSizeLogical = 32;          // logical (pre-DPI) tile size
    private string _mapPath = @"Assets/Maps/map1.json";

    // ---- UI ----
    private readonly MenuStrip _menu;
    private readonly CanvasPanel _canvas;       // <— custom flicker-free panel

    // ---- Data ----
    private TileMap _map;
    private int _selectedTileId = 0;

    public MainForm()
    {
        // Form setup (Designer-free)
        Text = "Tile Editor (Panel-based, Flicker-Free)";
        ClientSize = new Size(900, 700);
        AutoScaleMode = AutoScaleMode.None;   // stable pixel math (no autoscale surprises)
        DoubleBuffered = true;                // helps menu strip too
        KeyPreview = true;

        // Create controls
        _menu = new MenuStrip();
        var file = new ToolStripMenuItem("File");
        var save = new ToolStripMenuItem("Save", null, (_, __) => SaveMap());
        var load = new ToolStripMenuItem("Load", null, (_, __) => LoadMap());
        file.DropDownItems.Add(save);
        file.DropDownItems.Add(load);
        _menu.Items.Add(file);

        _canvas = new CanvasPanel
        {
            BackColor = Color.White
        };

        // ---- Option A: add canvas FIRST, then menu; set docks explicitly ----
        Controls.Add(_canvas);            // added first
        Controls.Add(_menu);              // added second (menu reserves top)
        MainMenuStrip = _menu;

        _menu.Dock = DockStyle.Top;
        _canvas.Dock = DockStyle.Fill;

        // Initialize map (20x15) with -1 = empty
        _map = new TileMap(20, 15, -1);

        // Wire events
        _canvas.Paint += Canvas_Paint;
        _canvas.MouseClick += Canvas_MouseClick;
        KeyDown += MainForm_KeyDown;

        // Seed a couple tiles so you see something immediately
        _map.Tiles[3, 3] = 0;
        _map.Tiles[3, 4] = 0;
        _map.Tiles[4, 3] = 0;
    }

    // Use DPI-aware size so drawing and hit-testing match on 125% / 150% scaling
    private int TileSizePx => (int)System.Math.Round(_tileSizeLogical * (DeviceDpi / 96f));

    private void Canvas_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

        var ts = TileSizePx;

        // Draw tiles + grid
        for (int y = 0; y < _map.Height; y++)
        {
            for (int x = 0; x < _map.Width; x++)
            {
                int id = _map.Tiles[y, x];
                Rectangle cell = new Rectangle(x * ts, y * ts, ts, ts);

                using var fill = new SolidBrush(id >= 0 ? Color.MediumSeaGreen : Color.Gainsboro);
                g.FillRectangle(fill, cell);
                g.DrawRectangle(Pens.Black, cell);
            }
        }

        // HUD text
        g.DrawString(
            $"Selected Tile: {_selectedTileId}   |   LMB=paint  RMB=erase   |   [ / ] change tile   |   S=Save  L=Load",
            Font, Brushes.Black, new PointF(8, 8));
    }

    private void Canvas_MouseClick(object? sender, MouseEventArgs e)
    {
        var ts = TileSizePx;

        // Mouse coordinates are relative to _canvas (no menu offset needed)
        int gridX = e.X / ts;
        int gridY = e.Y / ts;

        if (gridX < 0 || gridY < 0 || gridX >= _map.Width || gridY >= _map.Height)
            return;

        if (e.Button == MouseButtons.Left)
        {
            _map.Tiles[gridY, gridX] = _selectedTileId;
        }
        else if (e.Button == MouseButtons.Right)
        {
            _map.Tiles[gridY, gridX] = -1;
        }

        // Invalidate only the changed cell to minimize redraw work and avoid flicker
        _canvas.Invalidate(new Rectangle(gridX * ts, gridY * ts, ts, ts));
    }

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.S) SaveMap();
        if (e.KeyCode == Keys.L) LoadMap();

        // change selected tile id
        if (e.KeyCode == Keys.OemOpenBrackets)  // [
        {
            _selectedTileId = System.Math.Max(0, _selectedTileId - 1);
            _canvas.Invalidate();
        }
        if (e.KeyCode == Keys.OemCloseBrackets) // ]
        {
            _selectedTileId++;
            _canvas.Invalidate();
        }
    }

    private void SaveMap()
    {
        var full = Path.GetFullPath(_mapPath);
        Directory.CreateDirectory(Path.GetDirectoryName(full)!);
        _map.Save(full);
        MessageBox.Show($"Saved:\n{full}", "Map", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void LoadMap()
    {
        var full = Path.GetFullPath(_mapPath);
        if (!File.Exists(full))
        {
            MessageBox.Show($"Not found:\n{full}\n\nSave first or adjust _mapPath.", "Map", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        _map = TileMap.Load(full);
        _canvas.Invalidate();
    }
}
