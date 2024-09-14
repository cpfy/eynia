using System; // for Convert
using System.Collections.Generic; // for Dictionary

/*
管理需要持久化的配置参数，，并提供加载和保存的方法
*/

public class UserConfig
{
    // 若未找到配置文件，则使用默认值
    public decimal BreakIntervalTime { get; set; } = 35;
    public decimal BreakLengthTime { get; set; } = 4;
    public bool IsForceBreak { get; set; } = true;
    public string ForceBreakType { get; set; } = "一般强制";
    public decimal PostponeCount { get; set; } = 3;
    public bool IsAllowPostpone { get; set; } = true;
    public bool IsAllowShowAlert { get; set; } = false;

    public bool IsAllowAutoStart { get; set; } = false;

    public void LoadFromDictionary(Dictionary<string, object> data)
    {
        if (data.TryGetValue(nameof(BreakIntervalTime), out var breakIntervalTime))
            BreakIntervalTime = Convert.ToDecimal(breakIntervalTime);

        if(data.TryGetValue(nameof(BreakLengthTime), out var breakLengthTime))
            BreakLengthTime = Convert.ToDecimal(breakLengthTime);

        if(data.TryGetValue(nameof(IsForceBreak), out var isForceBreak))
            IsForceBreak = Convert.ToBoolean(isForceBreak);

        if(data.TryGetValue(nameof(ForceBreakType), out var forceBreakType))
            ForceBreakType = Convert.ToString(forceBreakType) ?? ForceBreakType;

        if(data.TryGetValue(nameof(PostponeCount), out var postponeCount))
            PostponeCount = Convert.ToDecimal(postponeCount);

        if(data.TryGetValue(nameof(IsAllowPostpone), out var isAllowPostpone))
            IsAllowPostpone = Convert.ToBoolean(isAllowPostpone);

        if(data.TryGetValue(nameof(IsAllowShowAlert), out var isAllowShowAlert))
            IsAllowShowAlert = Convert.ToBoolean(isAllowShowAlert);

        if(data.TryGetValue(nameof(IsAllowAutoStart), out var isAllowAutoStart))
            IsAllowAutoStart = Convert.ToBoolean(isAllowAutoStart);
    }

    public Dictionary<string, object> SaveToDictionary()
    {
        return new Dictionary<string, object>
        {
            { nameof(BreakIntervalTime), BreakIntervalTime },
            { nameof(BreakLengthTime), BreakLengthTime },
            { nameof(IsForceBreak), IsForceBreak },
            { nameof(ForceBreakType), ForceBreakType },
            { nameof(PostponeCount), PostponeCount },
            { nameof(IsAllowPostpone), IsAllowPostpone },
            { nameof(IsAllowShowAlert), IsAllowShowAlert },
            { nameof(IsAllowAutoStart), IsAllowAutoStart }
        };
    }

    // public updateDict()
}