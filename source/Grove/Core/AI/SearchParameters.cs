﻿namespace Grove.AI
{
  public class SearchParameters
  {
    public readonly int SearchDepth;
    public readonly int TargetCount;
    public readonly SearchPartitioningStrategy SearchPartitioningStrategy;

    public SearchParameters(int searchDepth, int targetCount,
      SearchPartitioningStrategy searchPartitioningStrategy)
    {
      SearchDepth = searchDepth;
      TargetCount = targetCount;
      SearchPartitioningStrategy = searchPartitioningStrategy;
    }

#if DEBUG
    public static SearchParameters Default = new SearchParameters(60, 2, SearchPartitioningStrategies.SingleThreaded);
#else 
    public static SearchParameters Default = new SearchParameters(60, 2, SearchPartitioningStrategies.DefaultMultithreaded);
#endif
  }
}