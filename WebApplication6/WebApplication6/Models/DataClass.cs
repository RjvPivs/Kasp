using System;
/// <summary>
/// Класс информации о сканировании.
/// </summary>
class DataClass
{
    /// <summary>
    /// Время сканирования.
    /// </summary>
    public TimeSpan Interval;
    /// <summary>
    /// Количество просканированных файлов.
    /// </summary>
    public int Amount;
    /// <summary>
    /// Проверка окончания сканирования.
    /// </summary>
    public bool Finished;
    /// <summary>
    /// Уникальный Id сканирования.
    /// </summary>
    public int Id;
    /// <summary>
    /// Путь сканируемой директории.
    /// </summary>
    public string Path;
    public (int, int, int, int) Mistakes;
    public DataClass(string path)
    {
        Amount = 0;
        Finished = false;
        Id = -1;
        Path = path;
        Mistakes = (0, 0, 0, 0);
    }
}
