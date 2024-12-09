#include "tcle_window.hpp"

namespace {
	int gWindowCount = 0;
}

namespace tcle {
	Window::Window(nullptr_t) {
		if (gWindowCount == 0) {
			if (!glfwInit()) return;
		}

		glfwWindowHint(GLFW_VISIBLE, GLFW_FALSE);
		mHandle = glfwCreateWindow(200, 200, "TCLE Native", nullptr, nullptr);

		if (!mHandle && gWindowCount == 0) {
			glfwTerminate();
			return;
		}

		++gWindowCount;
	}

	Window::~Window() noexcept {
		if (!mHandle) return;

		glfwDestroyWindow(mHandle);
		--gWindowCount;

		if (gWindowCount == 0) glfwTerminate();
	}
}