
#import "GetNice.h"
#import <UIKit/UIKit.h>

@interface GetNice ()

@end

@implementation GetNice


//弹出评分
void RequestNice()
{
	if([SKStoreReviewController respondsToSelector:@selector(requestReview)])
	{
    [SKStoreReviewController requestReview];
    }
	else
	{
    NSString  * nsStringToOpen = [NSString  stringWithFormat: @"itms-apps://itunes.apple.com/app/id%@?action=write-review",@"1410156910"];//对应的APPID
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString:nsStringToOpen]];
    }
}



@end
