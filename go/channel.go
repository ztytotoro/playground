package main

import (
	"sync"
	"time"
)

var wg sync.WaitGroup = sync.WaitGroup{}

type ReadWriteStream struct {
	ReadChan  chan int
	WriteChan chan int
}

func main() {
	rwq := &ReadWriteStream{
		ReadChan:  make(chan int, 100000),
		WriteChan: make(chan int, 100000),
	}
	wg.Add(4)
	go produceReadData(rwq, 10)
	go produceWriteData(rwq, 100)

	rCount := 0
	wCount := 0

	go func(r <-chan int, w <-chan int) {
		for {
			select {
			case <-r:
				rCount++
			case <-w:
				wCount++
			default:
				time.Sleep(time.Duration(time.Millisecond))
			}
		}
	}(rwq.ReadChan, rwq.WriteChan)

	go func() {
		timer1 := time.NewTicker(time.Second)

		for range timer1.C {
			println("Read Count:", rCount)
			println("Write Count:", wCount)
		}
	}()

	wg.Wait()
}

func produceReadData(rwq *ReadWriteStream, frq int) {
	timer1 := time.NewTicker(time.Millisecond * time.Duration(frq))
	for range timer1.C {
		rwq.ReadChan <- 0
	}
}

func produceWriteData(rwq *ReadWriteStream, frq int) {
	timer1 := time.NewTicker(time.Millisecond * time.Duration(frq))
	for range timer1.C {
		rwq.WriteChan <- 1
	}
}
