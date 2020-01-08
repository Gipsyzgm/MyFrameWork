#import "Achievement.h"
#import <GameKit/GameKit.h>

@implementation Achievement

- (void)reportAchievment:(NSString *)identifier withPercentageComplete:(double)percentComplete{
    NSLog(@"reportAchievment");
	NSLog(@"%@,%f",identifier,percentComplete);
    GKAchievement *achievement = [[GKAchievement alloc] initWithIdentifier:identifier];
    
    [achievement setPercentComplete:percentComplete];
     
    [achievement reportAchievementWithCompletionHandler:^(NSError *error) {
        if(error != nil){
            NSLog(@"error:%@", [error localizedDescription]);
        }else{
            NSString *str = [NSString stringWithFormat:@"%f",percentComplete];
            NSString *str2 = [[identifier stringByAppendingString:@":"]stringByAppendingString:str];
            UnitySendMessage("IAP", "AchievementSucc", [str2 UTF8String]);
            NSLog(@"reportAchievmentSucc:%@",str2);
			
        }
    }];
}

void reportAchievmentDo(id *p)
{
    NSLog(@"reportAchievmentDo");
    NSString * string = [NSString stringWithUTF8String:p];
    
    NSArray *array = [string componentsSeparatedByString:@":"];
    NSLog(@"array:%@",array);
    
    Achievement *vc = [[Achievement alloc]init];
    [vc reportAchievment:array[0] withPercentageComplete:[array[1] doubleValue]];
    
    
}

@end
