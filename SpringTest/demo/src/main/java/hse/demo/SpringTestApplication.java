package hse.demo;

import java.util.ArrayList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;

@SpringBootApplication
@RestController
public class SpringTestApplication {
	@Autowired ItemRepository itemRepository;

	public static void main(String[] args) {
		SpringApplication.run(SpringTestApplication.class, args);
	}

	@PostMapping(path = "/item", consumes = "application/json", produces = "application/json")
	public Item createItem(@RequestBody Item item) {
		if (!itemRepository.existsById(item.getName())) {
			itemRepository.save(item);
			return item;
		}
		else {
			return new Item("item already exists", 0);
		}
	}

	@GetMapping(path = "/item", produces = "application/json")
	public List<Item> getItems() {
		List<Item> list = new ArrayList<>();
		itemRepository.findAll().forEach(list::add);;
		return list;
	}

	@DeleteMapping(path = "/item")
	public void deleteItem(@RequestBody String name) {
		itemRepository.deleteById(name);
	}

	@PutMapping(path = "/item", consumes = "application/json", produces = "application/json")
	public Item updateItem(@RequestBody Item item) {
		if (itemRepository.existsById(item.getName())) {
			itemRepository.save(item);
			return item;
		}
		else {
			return new Item("item does not exist", 0);
		}
	}

}
