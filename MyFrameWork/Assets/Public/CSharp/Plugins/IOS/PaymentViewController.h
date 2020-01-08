//  
//  PaymentViewController.h  
//  IAPPayTest  
//  
//  Created by silicon on 14-10-28.  
//  Copyright (c) 2014年 silicon. All rights reserved.  
//  
  
#import <UIKit/UIKit.h>  
  
#import <StoreKit/StoreKit.h>  
  
@interface PaymentViewController : UIViewController<SKPaymentTransactionObserver,SKProductsRequestDelegate>  
  
@end