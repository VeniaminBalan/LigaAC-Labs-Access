using Microsoft.Extensions.VectorData;

namespace Api.Models.VectorSearch;

public class Text
{
    [VectorStoreRecordKey]
    public required string Id { get; set; }

    [VectorStoreRecordData(IsFullTextSearchable = false)]
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
    public string DocumentPath { get; set; } = string.Empty;

    [VectorStoreRecordData(IsFullTextSearchable = true)]
    public string ActualText { get; set; } = string.Empty;

    [VectorStoreRecordVector(Dimensions: 1024, DistanceFunction.CosineSimilarity, IndexKind.Hnsw)]
    public ReadOnlyMemory<float>? DescriptionEmbedding { get; set; }
}