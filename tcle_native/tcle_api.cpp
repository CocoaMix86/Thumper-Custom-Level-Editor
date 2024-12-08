#include <cstdlib>

#define TCLE_OK 1

struct TCLEGlobalData final {
    char* pixelBuffer = nullptr;
};

static TCLEGlobalData gData;

static int iteration = 0;

extern "C" __declspec(dllexport) void* __cdecl tcle_native_draw(int width, int height) {
    // Safety
    if (width < 1 || height < 1) return nullptr;

    ++iteration;

    if (gData.pixelBuffer) free(gData.pixelBuffer);
    gData.pixelBuffer = reinterpret_cast<char*>(malloc(width * height * 4));

    for (int y = 0; y < height; ++y) {
        for (int x = 0; x < width; ++x) {
            gData.pixelBuffer[y * width * 4 + x * 4 + 0] = x + iteration * 20;
            gData.pixelBuffer[y * width * 4 + x * 4 + 1] = y;
            gData.pixelBuffer[y * width * 4 + x * 4 + 2] = x + y;
            gData.pixelBuffer[y * width * 4 + x * 4 + 3] = 255;
        }
    }

    gData.pixelBuffer[(height / 2) * width * 4 + (width / 2) * 4 + 0] = 0;
    gData.pixelBuffer[(height / 2) * width * 4 + (width / 2) * 4 + 1] = 0;
    gData.pixelBuffer[(height / 2) * width * 4 + (width / 2) * 4 + 2] = 0;
    gData.pixelBuffer[(height / 2) * width * 4 + (width / 2) * 4 + 3] = 255;

    return gData.pixelBuffer;
}

extern "C" __declspec(dllexport) int __cdecl tcle_native_init(void) {
    return TCLE_OK;
}

extern "C" __declspec(dllexport) void __cdecl tcle_native_uninit(void) {
    if (gData.pixelBuffer) free(gData.pixelBuffer);
}