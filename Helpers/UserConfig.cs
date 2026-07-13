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

    // 家长控制 (每日屏幕限时)
    public bool IsEnableDailyLimit { get; set; } = false;
    public decimal DailyLimitTime { get; set; } = 150; // 默认2.5小时
    public string DailyLimitDate { get; set; } = "";
    public double DailyLimitAccumulatedSeconds { get; set; } = 0;
    public string ParentalControlStyle { get; set; } = "锁定只读";

    // 外观
    // public string BubbleSize { get; set; } = "中";
    public double UIScale { get; set; } = 1.0; // 界面缩放比例

    // 高级
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

        // 家长控制
        if(data.TryGetValue(nameof(IsEnableDailyLimit), out var isEnableDailyLimit))
            IsEnableDailyLimit = Convert.ToBoolean(isEnableDailyLimit);

        if(data.TryGetValue(nameof(DailyLimitTime), out var dailyLimitTime))
            DailyLimitTime = Convert.ToDecimal(dailyLimitTime);

        if(data.TryGetValue(nameof(DailyLimitDate), out var dailyLimitDate))
            DailyLimitDate = Convert.ToString(dailyLimitDate) ?? "";

        if(data.TryGetValue(nameof(DailyLimitAccumulatedSeconds), out var dailyLimitAccumulatedSeconds))
            DailyLimitAccumulatedSeconds = Convert.ToDouble(dailyLimitAccumulatedSeconds);

        if(data.TryGetValue(nameof(ParentalControlStyle), out var parentalControlStyle))
            ParentalControlStyle = Convert.ToString(parentalControlStyle) ?? "锁定只读";

        // ------------------------------

        // if(data.TryGetValue(nameof(BubbleSize), out var bubbleSize))
        //     BubbleSize = Convert.ToString(bubbleSize) ?? BubbleSize;
        if(data.TryGetValue(nameof(UIScale), out var uiScale))
            UIScale = Convert.ToDouble(uiScale);

        // ------------------------------

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
            // 家长控制
            { nameof(IsEnableDailyLimit), IsEnableDailyLimit },
            { nameof(DailyLimitTime), DailyLimitTime },
            { nameof(DailyLimitDate), DailyLimitDate },
            { nameof(DailyLimitAccumulatedSeconds), DailyLimitAccumulatedSeconds },
            { nameof(ParentalControlStyle), ParentalControlStyle },
            // { nameof(BubbleSize), BubbleSize },
            { nameof(UIScale), UIScale },
            { nameof(IsAllowAutoStart), IsAllowAutoStart }
        };
    }

    // public updateDict()
}