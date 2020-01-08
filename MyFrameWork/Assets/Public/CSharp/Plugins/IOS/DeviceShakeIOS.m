
#import "DeviceShakeIOS.h"

@implementation DeviceShakeIOS

#if defined(__cplusplus)
extern "C"{   
#endif
    //只支持Iphone7以上的手机
	void setVibratorIOS()
	{
		UIImpactFeedbackGenerator *feedBackGenertor = [[UIImpactFeedbackGenerator alloc]initWithStyle:UIImpactFeedbackStyleLight];
		[feedBackGenertor impactOccurred];
	}

}
    
#if defined(__cplusplus)   
}
#endif
