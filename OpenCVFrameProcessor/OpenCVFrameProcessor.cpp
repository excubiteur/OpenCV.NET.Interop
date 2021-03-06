#include<cstdint>

#include<opencv2/opencv.hpp>

typedef std::int32_t(*CallbackType)(std::uint8_t * bytes, std::int32_t, std::int32_t, std::int32_t);

static CallbackType s_callback;

extern "C"
{

	__declspec(dllexport) std::int16_t __cdecl ProcessFrame(std::uint8_t * bytes, std::int32_t height, std::int32_t width)
	{
		using namespace cv;
		Mat image(height, width, CV_8UC4, bytes);
		Mat result;
		resize(image, result, Size(), 0.1, 0.1);
		auto size = result.rows*result.cols * 4;
		s_callback(result.data,size, result.rows, result.cols);
		return 0;
	}

	__declspec(dllexport) void __cdecl SetCallback(CallbackType callback)
	{
		s_callback = callback;
	}
}