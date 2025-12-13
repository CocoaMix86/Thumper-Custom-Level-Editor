#pragma once

#include <glad/gl.h>
#include <GLFW/glfw3.h>

#include <utility>

namespace tcle {
	class Window final {
	public:
		Window() = default;
		explicit Window(nullptr_t);
		~Window() noexcept;
		Window(Window const&) = delete;
		Window& operator=(Window const&) = delete;
		inline Window(Window&& other) noexcept { *this = std::move(other); }
		inline Window& operator=(Window&& other) noexcept { std::swap(mHandle, other.mHandle); return *this; }

		inline GLFWwindow* handle() const { return mHandle; }
	private:
		GLFWwindow* mHandle = nullptr;
	};
}