using IndekserPrzypraw.Services;

namespace IndekserPrzyprawTests;

public class LocalSpiceServiceTest
{
    [Fact]
    public async void GetAllSpicesAsync_ReturnsAllSpice()
    {
        var spiceService = new LocalSpiceService();
    }
}