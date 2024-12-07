#define TCLE_OK 1

extern "C" __declspec(dllexport) int __cdecl tcle_native_init(void) {
	return TCLE_OK;
}

extern "C" __declspec(dllexport) void __cdecl tcle_native_uninit(void) {
}