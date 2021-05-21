use futures::executor;

fn main() {
    executor::block_on(async_test());
}

async fn async_test() {
    println!("Hello, world!");
}