use futures::executor;

pub fn test() {
    executor::block_on(async_test());
}

async fn async_test() {
    println!("Hello, world!");
}